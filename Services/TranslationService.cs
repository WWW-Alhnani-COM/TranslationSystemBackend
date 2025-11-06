using Microsoft.EntityFrameworkCore;
using TranslationDB.Data;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Services
{
    public class TranslationService : BaseService, ITranslationService
    {
        private readonly AppDbContext _context;

        public TranslationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Translation>>> GetAllTranslations()
        {
            try
            {
                var translations = await _context.Translations
                    .Include(t => t.Paragraph)
                    .Include(t => t.Translator)
                    .Include(t => t.Reviews)
                    .OrderByDescending(t => t.TranslatedAt)
                    .ToListAsync();

                return Success(translations);
            }
            catch (Exception ex)
            {
                return Error<List<Translation>>($"Error retrieving translations: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Translation>> GetTranslationById(string id)
        {
            try
            {
                var translation = await _context.Translations
                    .Include(t => t.Paragraph)
                    .Include(t => t.Translator)
                    .Include(t => t.Reviews)
                        .ThenInclude(r => r.Reviewer)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (translation == null)
                {
                    return Error<Translation>("Translation not found");
                }

                return Success(translation);
            }
            catch (Exception ex)
            {
                return Error<Translation>($"Error retrieving translation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Translation>> CreateTranslation(Translation translation)
        {
            try
            {
                // Set default values
                translation.Id = Guid.NewGuid().ToString();
                translation.TranslatedAt = DateTime.UtcNow;
                translation.Version = 1;
                translation.Status = Enums.TranslationStatus.Draft;

                _context.Translations.Add(translation);
                await _context.SaveChangesAsync();

                return Success(translation, "Translation created successfully");
            }
            catch (Exception ex)
            {
                return Error<Translation>($"Error creating translation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Translation>> UpdateTranslation(Translation translation)
        {
            try
            {
                var existingTranslation = await _context.Translations.FindAsync(translation.Id);
                if (existingTranslation == null)
                {
                    return Error<Translation>("Translation not found");
                }

                // Update properties
                existingTranslation.TranslatedText = translation.TranslatedText;
                existingTranslation.Version = translation.Version;
                existingTranslation.Status = translation.Status;

                _context.Translations.Update(existingTranslation);
                await _context.SaveChangesAsync();

                return Success(existingTranslation, "Translation updated successfully");
            }
            catch (Exception ex)
            {
                return Error<Translation>($"Error updating translation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<bool>> DeleteTranslation(string id)
        {
            try
            {
                var translation = await _context.Translations.FindAsync(id);
                if (translation == null)
                {
                    return Error<bool>("Translation not found");
                }

                _context.Translations.Remove(translation);
                await _context.SaveChangesAsync();

                return Success(true, "Translation deleted successfully");
            }
            catch (Exception ex)
            {
                return Error<bool>($"Error deleting translation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Translation>> SubmitTranslation(string id)
        {
            try
            {
                var translation = await _context.Translations.FindAsync(id);
                if (translation == null)
                {
                    return Error<Translation>("Translation not found");
                }

                translation.Status = Enums.TranslationStatus.Submitted;
                translation.SubmittedAt = DateTime.UtcNow;

                _context.Translations.Update(translation);
                await _context.SaveChangesAsync();

                return Success(translation, "Translation submitted successfully");
            }
            catch (Exception ex)
            {
                return Error<Translation>($"Error submitting translation: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Review>>> GetTranslationReviews(string translationId)
        {
            try
            {
                var reviews = await _context.Reviews
                    .Where(r => r.TranslationId == translationId)
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

        public async Task<ServiceResult<List<Translation>>> GetUserTranslations(string userId)
        {
            try
            {
                var translations = await _context.Translations
                    .Where(t => t.TranslatorId == userId)
                    .Include(t => t.Paragraph)
                        .ThenInclude(p => p.Project)
                    .OrderByDescending(t => t.TranslatedAt)
                    .ToListAsync();

                return Success(translations);
            }
            catch (Exception ex)
            {
                return Error<List<Translation>>($"Error retrieving user translations: {ex.Message}");
            }
        }
    }
}