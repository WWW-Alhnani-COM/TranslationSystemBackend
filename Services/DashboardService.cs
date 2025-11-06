using Microsoft.EntityFrameworkCore;
using TranslationDB.Data;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;
using TranslationManagementSystem.API.Models.DTOs;

namespace TranslationDB.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<DashboardStatsDto>> GetDashboardStats(string userId, string userRole)
        {
            try
            {
                var stats = new DashboardStatsDto();

                switch (userRole)
                {
                    case "SystemAdmin":
                        stats.TotalProjects = await _context.Projects.CountAsync();
                        stats.CompletedProjects = await _context.Projects.CountAsync(p => p.Status == Enums.ProjectStatus.Completed);
                        stats.InTranslation = await _context.Projects.CountAsync(p => p.Status == Enums.ProjectStatus.InTranslation);
                        stats.InReview = await _context.Projects.CountAsync(p => p.Status == Enums.ProjectStatus.InReview);
                        stats.PendingApproval = await _context.Projects.CountAsync(p => p.Status == Enums.ProjectStatus.PendingApproval);
                        stats.TotalWordsTranslated = await CalculateTotalWordsTranslated();
                        stats.AvgQualityScore = await CalculateAverageQualityScore();
                        break;

                    case "Translator":
                        var translatorProjects = await _context.ProjectAssignments
                            .Where(pa => pa.UserId == userId && pa.AssignmentType == "Translator")
                            .Select(pa => pa.ProjectId)
                            .ToListAsync();

                        stats.TotalProjects = await _context.Projects
                            .CountAsync(p => translatorProjects.Contains(p.Id) && 
                                           (p.Status == Enums.ProjectStatus.InTranslation || 
                                            p.Status == Enums.ProjectStatus.InReview || 
                                            p.Status == Enums.ProjectStatus.PendingApproval || 
                                            p.Status == Enums.ProjectStatus.Completed));

                        stats.CompletedProjects = await _context.Projects
                            .CountAsync(p => translatorProjects.Contains(p.Id) && 
                                           p.Status == Enums.ProjectStatus.Completed);

                        stats.ActiveTasks = await _context.Translations
                            .CountAsync(t => t.TranslatorId == userId && 
                                           t.Status != Enums.TranslationStatus.Approved);

                        stats.ApprovedTasks = await _context.Translations
                            .CountAsync(t => t.TranslatorId == userId && 
                                           t.Status == Enums.TranslationStatus.Approved);

                        stats.TotalWordsTranslated = await CalculateUserWordsTranslated(userId);
                        stats.AvgQualityScore = await CalculateUserAverageQualityScore(userId);
                        break;

                    case "Reviewer":
                        var reviewerProjects = await _context.ProjectAssignments
                            .Where(pa => pa.UserId == userId && pa.AssignmentType == "Reviewer")
                            .Select(pa => pa.ProjectId)
                            .ToListAsync();

                        stats.TotalProjects = await _context.Projects
                            .CountAsync(p => reviewerProjects.Contains(p.Id) && 
                                           (p.Status == Enums.ProjectStatus.InReview || 
                                            p.Status == Enums.ProjectStatus.PendingApproval || 
                                            p.Status == Enums.ProjectStatus.Completed));

                        stats.CompletedProjects = await _context.Projects
                            .CountAsync(p => reviewerProjects.Contains(p.Id) && 
                                           p.Status == Enums.ProjectStatus.Completed);

                        stats.PendingReviews = await _context.Reviews
                            .CountAsync(r => r.ReviewerId == userId && 
                                           r.Status == Enums.ReviewStatus.Pending);

                        stats.CompletedReviews = await _context.Reviews
                            .CountAsync(r => r.ReviewerId == userId && 
                                           (r.Status == Enums.ReviewStatus.Approved || 
                                            r.Status == Enums.ReviewStatus.NeedsRevision || 
                                            r.Status == Enums.ReviewStatus.Rejected));

                        stats.AvgReviewTime = await CalculateAverageReviewTime(userId);
                        stats.AvgQualityScore = await CalculateReviewerAverageQualityScore(userId);
                        break;

                    default: // DataEntry, Supervisor, etc.
                        stats.TotalProjects = await _context.Projects.CountAsync(p => p.CreatedBy == userId);
                        stats.CompletedProjects = await _context.Projects.CountAsync(p => p.CreatedBy == userId && p.Status == Enums.ProjectStatus.Completed);
                        stats.InTranslation = await _context.Projects.CountAsync(p => p.CreatedBy == userId && p.Status == Enums.ProjectStatus.InTranslation);
                        stats.InReview = await _context.Projects.CountAsync(p => p.CreatedBy == userId && p.Status == Enums.ProjectStatus.InReview);
                        stats.PendingApproval = await _context.Projects.CountAsync(p => p.CreatedBy == userId && p.Status == Enums.ProjectStatus.PendingApproval);
                        break;
                }

                return Success(stats);
            }
            catch (Exception ex)
            {
                return Error<DashboardStatsDto>($"Error retrieving dashboard stats: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<object>>> GetRecentActivities()
        {
            try
            {
                var activities = new List<object>();

                // Recent projects
                var recentProjects = await _context.Projects
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(5)
                    .Select(p => new { Type = "Project", Action = "Created", p.Name, p.CreatedAt })
                    .ToListAsync<object>();

                // Recent translations
                var recentTranslations = await _context.Translations
                    .Include(t => t.Translator)
                    .OrderByDescending(t => t.TranslatedAt)
                    .Take(5)
                    .Select(t => new { Type = "Translation", Action = "Submitted", Translator = t.Translator.FullName, t.TranslatedAt })
                    .ToListAsync<object>();

                // Recent reviews
                var recentReviews = await _context.Reviews
                    .Include(r => r.Reviewer)
                    .OrderByDescending(r => r.ReviewedAt)
                    .Take(5)
                    .Select(r => new { Type = "Review", Action = r.Status.ToString(), Reviewer = r.Reviewer.FullName, r.ReviewedAt })
                    .ToListAsync<object>();

                activities.AddRange(recentProjects);
                activities.AddRange(recentTranslations);
                activities.AddRange(recentReviews);

                // Sort by date and take top 10
                var sortedActivities = activities
                    .OrderByDescending(a => GetActivityDate(a))
                    .Take(10)
                    .ToList();

                return Success(sortedActivities);
            }
            catch (Exception ex)
            {
                return Error<List<object>>($"Error retrieving recent activities: {ex.Message}");
            }
        }

        public async Task<ServiceResult<object>> GetProjectProgress(int projectId)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.Paragraphs)
                    .FirstOrDefaultAsync(p => p.Id == projectId);

                if (project == null)
                {
                    return Error<object>("Project not found");
                }

                var totalParagraphs = project.Paragraphs.Count;
                var translatedParagraphs = project.Paragraphs.Count(p => 
                    p.Translations.Any(t => t.Status == Enums.TranslationStatus.Approved));

                var progress = new
                {
                    ProjectId = projectId,
                    ProjectName = project.Name,
                    TotalParagraphs = totalParagraphs,
                    TranslatedParagraphs = translatedParagraphs,
                    ProgressPercentage = totalParagraphs > 0 ? (translatedParagraphs * 100) / totalParagraphs : 0
                };

                return Success<object>(progress);
            }
            catch (Exception ex)
            {
                return Error<object>($"Error retrieving project progress: {ex.Message}");
            }
        }

        private DateTime GetActivityDate(object activity)
        {
            var type = activity.GetType();
            var createdAt = type.GetProperty("CreatedAt")?.GetValue(activity) as DateTime?;
            var translatedAt = type.GetProperty("TranslatedAt")?.GetValue(activity) as DateTime?;
            var reviewedAt = type.GetProperty("ReviewedAt")?.GetValue(activity) as DateTime?;

            return createdAt ?? translatedAt ?? reviewedAt ?? DateTime.MinValue;
        }

        private async Task<int> CalculateTotalWordsTranslated()
        {
            var approvedTranslations = await _context.Translations
                .Where(t => t.Status == Enums.TranslationStatus.Approved)
                .ToListAsync();

            return approvedTranslations.Sum(t => CountWords(t.TranslatedText));
        }

        private async Task<int> CalculateUserWordsTranslated(string userId)
        {
            var userTranslations = await _context.Translations
                .Where(t => t.TranslatorId == userId && t.Status == Enums.TranslationStatus.Approved)
                .ToListAsync();

            return userTranslations.Sum(t => CountWords(t.TranslatedText));
        }

        private async Task<double> CalculateAverageQualityScore()
        {
            var ratings = await _context.Reviews
                .Where(r => r.Rating.HasValue)
                .Select(r => r.Rating.Value)
                .ToListAsync();

            return ratings.Any() ? ratings.Average() : 0;
        }

        private async Task<double> CalculateUserAverageQualityScore(string userId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.Rating.HasValue && r.Translation.TranslatorId == userId)
                .Select(r => r.Rating.Value)
                .ToListAsync();

            return ratings.Any() ? ratings.Average() : 0;
        }

        private async Task<double> CalculateReviewerAverageQualityScore(string userId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.Rating.HasValue && r.ReviewerId == userId)
                .Select(r => r.Rating.Value)
                .ToListAsync();

            return ratings.Any() ? ratings.Average() : 0;
        }

        private async Task<double> CalculateAverageReviewTime(string userId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.Translation)
                .Where(r => r.ReviewerId == userId && r.Translation.SubmittedAt.HasValue && r.ReviewedAt != null)
                .Select(r => new { r.Translation.SubmittedAt, r.ReviewedAt })
                .ToListAsync();

            if (!reviews.Any()) return 0;

            var totalHours = reviews.Sum(r => (r.ReviewedAt - r.SubmittedAt.Value).TotalHours);
            return totalHours / reviews.Count;
        }

        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            var wordCount = 0;
            var inWord = false;

            foreach (var c in text)
            {
                if (char.IsWhiteSpace(c))
                {
                    inWord = false;
                }
                else if (!inWord)
                {
                    wordCount++;
                    inWord = true;
                }
            }

            return wordCount;
        }
    }
}