using Enums = TranslationDB.Enums;
using Microsoft.EntityFrameworkCore;
using TranslationDB.Data;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Services
{
    public class ReviewService : BaseService, IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Review>>> GetAllReviews()
        {
            try
            {
                var reviews = await _context.Reviews
                    .Include(r => r.Translation)
                    .Include(r => r.Reviewer)
                    .OrderByDescending(r => r.ReviewedAt)
                    .ToListAsync();

                return Success(reviews);
            }
            catch (Exception ex)
            {
                return Error<List<Review>>($"Error retrieving reviews: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Review>> GetReviewById(string id)
        {
            try
            {
                var review = await _context.Reviews
                    .Include(r => r.Translation)
                        .ThenInclude(t => t.Translator)
                    .Include(r => r.Reviewer)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (review == null)
                {
                    return Error<Review>("Review not found");
                }

                return Success(review);
            }
            catch (Exception ex)
            {
                return Error<Review>($"Error retrieving review: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Review>> CreateReview(Review review)
        {
            try
            {
                // Set default values
                review.Id = Guid.NewGuid().ToString();
                review.ReviewedAt = DateTime.UtcNow;
                review.Status = Enums.ReviewStatus.Pending;

                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();

                return Success(review, "Review created successfully");
            }
            catch (Exception ex)
            {
                return Error<Review>($"Error creating review: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Review>> UpdateReview(Review review)
        {
            try
            {
                var existingReview = await _context.Reviews.FindAsync(review.Id);
                if (existingReview == null)
                {
                    return Error<Review>("Review not found");
                }

                // Update properties
                existingReview.ReviewedText = review.ReviewedText;
                existingReview.Comments = review.Comments;
                existingReview.Rating = review.Rating;
                existingReview.Status = review.Status;

                _context.Reviews.Update(existingReview);
                await _context.SaveChangesAsync();

                return Success(existingReview, "Review updated successfully");
            }
            catch (Exception ex)
            {
                return Error<Review>($"Error updating review: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteReview(string id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return Error<bool>("Review not found");
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return Success(true, "Review deleted successfully");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Error deleting review: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Review>> ApproveReview(string id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return Error<Review>("Review not found");
                }

                review.Status = Enums.ReviewStatus.Approved;
                review.ReviewedAt = DateTime.UtcNow;

                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();

                return Success(review, "Review approved successfully");
            }
            catch (Exception ex)
            {
                return Error<Review>($"Error approving review: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Review>> RejectReview(string id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return Error<Review>("Review not found");
                }

                review.Status = Enums.ReviewStatus.Rejected;
                review.ReviewedAt = DateTime.UtcNow;

                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();

                return Success(review, "Review rejected successfully");
            }
            catch (Exception ex)
            {
                return Error<Review>($"Error rejecting review: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Review>> RequestRevision(string id, string comments)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return Error<Review>("Review not found");
                }

                review.Status = Enums.ReviewStatus.NeedsRevision;
                review.Comments = comments;
                review.ReviewedAt = DateTime.UtcNow;

                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();

                return Success(review, "Revision requested successfully");
            }
            catch (Exception ex)
            {
                return Error<Review>($"Error requesting revision: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Review>>> GetUserReviews(string userId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Where(r => r.ReviewerId == userId)
                    .Include(r => r.Translation)
                        .ThenInclude(t => t.Paragraph)
                    .OrderByDescending(r => r.ReviewedAt)
                    .ToListAsync();

                return Success(reviews);
            }
            catch (Exception ex)
            {
                return Error<List<Review>>($"Error retrieving user reviews: {ex.Message}");
            }
        }
    }
}