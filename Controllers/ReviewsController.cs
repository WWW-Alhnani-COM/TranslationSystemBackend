using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Controllers
{
    public class ReviewsController : BaseController
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var result = await _reviewService.GetAllReviews();
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving reviews: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(string id)
        {
            try
            {
                var result = await _reviewService.GetReviewById(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving review: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(Review review)
        {
            try
            {
                var result = await _reviewService.CreateReview(review);
                if (!result.Success)
                    return HandleError(result.Message);

                return CreatedAtAction(nameof(GetReviewById), new { id = result.Data.Id }, 
                    new { success = true, data = result.Data, message = "Review created successfully" });
            }
            catch (Exception ex)
            {
                return HandleError($"Error creating review: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(string id, Review review)
        {
            try
            {
                if (id != review.Id)
                    return HandleError("ID mismatch");

                var result = await _reviewService.UpdateReview(review);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Review updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error updating review: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                var result = await _reviewService.DeleteReview(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult<object?>(null, "Review deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error deleting review: {ex.Message}");
            }
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveReview(string id)
        {
            try
            {
                var result = await _reviewService.ApproveReview(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Review approved successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error approving review: {ex.Message}");
            }
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectReview(string id)
        {
            try
            {
                var result = await _reviewService.RejectReview(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Review rejected successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error rejecting review: {ex.Message}");
            }
        }

        [HttpPost("{id}/request-revision")]
        public async Task<IActionResult> RequestRevision(string id, [FromBody] RevisionRequestDto revisionRequest)
        {
            try
            {
                var result = await _reviewService.RequestRevision(id, revisionRequest.Comments);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Revision requested successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error requesting revision: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserReviews(string userId)
        {
            try
            {
                var result = await _reviewService.GetUserReviews(userId);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving user reviews: {ex.Message}");
            }
        }
    }
}