using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models;
using TranslationDB.Services;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Controllers
{
    public class TranslationsController : BaseController
    {
        private readonly ITranslationService _translationService;

        public TranslationsController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTranslations()
        {
            try
            {
                var translations = await _translationService.GetAllTranslations();
                return HandleResult(translations);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving translations: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTranslationById(string id)
        {
            try
            {
                var translation = await _translationService.GetTranslationById(id);
                return HandleResult(translation);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving translation: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTranslation(Translation translation)
        {
            try
            {
                var result = await _translationService.CreateTranslation(translation);
                if (!result.Success)
                    return HandleError(result.Message);

                return CreatedAtAction(nameof(GetTranslationById), new { id = result.Data.Id }, 
                    new { success = true, data = result.Data, message = "Translation created successfully" });
            }
            catch (Exception ex)
            {
                return HandleError($"Error creating translation: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTranslation(string id, Translation translation)
        {
            try
            {
                if (id != translation.Id)
                    return HandleError("ID mismatch");

                var result = await _translationService.UpdateTranslation(translation);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Translation updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error updating translation: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranslation(string id)
        {
            try
            {
                var result = await _translationService.DeleteTranslation(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult<object?>(null, "Translation deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error deleting translation: {ex.Message}");
            }
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitTranslation(string id)
        {
            try
            {
                var result = await _translationService.SubmitTranslation(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Translation submitted successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error submitting translation: {ex.Message}");
            }
        }

        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetTranslationReviews(string id)
        {
            try
            {
                var reviews = await _translationService.GetTranslationReviews(id);
                return HandleResult(reviews);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving reviews: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserTranslations(string userId)
        {
            try
            {
                var translations = await _translationService.GetUserTranslations(userId);
                return HandleResult(translations);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving user translations: {ex.Message}");
            }
        }
    }
}