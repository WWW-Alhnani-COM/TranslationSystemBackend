using TranslationDB.Models;
using TranslationDB.Models.DTOs;

namespace TranslationDB.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ServiceResult<List<Review>>> GetAllReviews();
        Task<ServiceResult<Review>> GetReviewById(string id);
        Task<ServiceResult<Review>> CreateReview(Review review);
        Task<ServiceResult<Review>> UpdateReview(Review review);
        Task<ServiceResult<bool>> DeleteReview(string id);
        Task<ServiceResult<Review>> ApproveReview(string id);
        Task<ServiceResult<Review>> RejectReview(string id);
        Task<ServiceResult<Review>> RequestRevision(string id, string comments);
        Task<ServiceResult<List<Review>>> GetUserReviews(string userId);
    }
}