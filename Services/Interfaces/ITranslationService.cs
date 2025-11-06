using TranslationDB.Models;
using TranslationDB.Models.DTOs;

namespace TranslationDB.Services.Interfaces
{
    public interface ITranslationService
    {
        Task<ServiceResult<List<Translation>>> GetAllTranslations();
        Task<ServiceResult<Translation>> GetTranslationById(string id);
        Task<ServiceResult<Translation>> CreateTranslation(Translation translation);
        Task<ServiceResult<Translation>> UpdateTranslation(Translation translation);
        Task<ServiceResult<bool>> DeleteTranslation(string id);
        Task<ServiceResult<Translation>> SubmitTranslation(string id);
        Task<ServiceResult<List<Review>>> GetTranslationReviews(string translationId);
        Task<ServiceResult<List<Translation>>> GetUserTranslations(string userId);
    }
}