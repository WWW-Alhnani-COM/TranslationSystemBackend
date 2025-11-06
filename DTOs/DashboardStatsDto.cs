namespace TranslationManagementSystem.API.Models.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int InTranslation { get; set; }
        public int InReview { get; set; }
        public int PendingApproval { get; set; }
        public int TotalWordsTranslated { get; set; }
        public double AvgQualityScore { get; set; }
        public int ActiveTasks { get; set; }
        public int ApprovedTasks { get; set; }
        public int PendingReviews { get; set; }
        public int CompletedReviews { get; set; }
        public double AvgReviewTime { get; set; }
    }
}