using Microsoft.EntityFrameworkCore;
using TranslationDB.Data;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Services
{
    public class ParagraphService : BaseService, IParagraphService
    {
        private readonly AppDbContext _context;

        public ParagraphService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Paragraph>>> GetAllParagraphs()
        {
            try
            {
                var paragraphs = await _context.Paragraphs
                    .Include(p => p.Project)
                    .Include(p => p.LockedByUser)
                    .OrderBy(p => p.ProjectId)
                    .ThenBy(p => p.Order)
                    .ToListAsync();

                return Success(paragraphs);
            }
            catch (Exception ex)
            {
                return Error<List<Paragraph>>($"Error retrieving paragraphs: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Paragraph>> GetParagraphById(int id)
        {
            try
            {
                var paragraph = await _context.Paragraphs
                    .Include(p => p.Project)
                    .Include(p => p.LockedByUser)
                    .Include(p => p.Translations)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (paragraph == null)
                {
                    return Error<Paragraph>("Paragraph not found");
                }

                return Success(paragraph);
            }
            catch (Exception ex)
            {
                return Error<Paragraph>($"Error retrieving paragraph: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Paragraph>> UpdateParagraph(Paragraph paragraph)
        {
            try
            {
                var existingParagraph = await _context.Paragraphs.FindAsync(paragraph.Id);
                if (existingParagraph == null)
                {
                    return Error<Paragraph>("Paragraph not found");
                }

                // Update properties
                existingParagraph.Order = paragraph.Order;
                existingParagraph.OriginalText = paragraph.OriginalText;
                existingParagraph.Status = paragraph.Status;
                existingParagraph.UpdatedAt = DateTime.UtcNow;

                _context.Paragraphs.Update(existingParagraph);
                await _context.SaveChangesAsync();

                return Success(existingParagraph, "Paragraph updated successfully");
            }
            catch (Exception ex)
            {
                return Error<Paragraph>($"Error updating paragraph: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Paragraph>> LockParagraph(int paragraphId, string userId)
        {
            try
            {
                var paragraph = await _context.Paragraphs.FindAsync(paragraphId);
                if (paragraph == null)
                {
                    return Error<Paragraph>("Paragraph not found");
                }

                if (paragraph.IsLocked && paragraph.LockedBy != userId)
                {
                    return Error<Paragraph>("Paragraph is already locked by another user");
                }

                paragraph.IsLocked = true;
                paragraph.LockedBy = userId;
                paragraph.LockedAt = DateTime.UtcNow;
                paragraph.UpdatedAt = DateTime.UtcNow;

                _context.Paragraphs.Update(paragraph);
                await _context.SaveChangesAsync();

                return Success(paragraph, "Paragraph locked successfully");
            }
            catch (Exception ex)
            {
                return Error<Paragraph>($"Error locking paragraph: {ex.Message}");
            }
        }

        public async Task<ServiceResult<Paragraph>> UnlockParagraph(int paragraphId)
        {
            try
            {
                var paragraph = await _context.Paragraphs.FindAsync(paragraphId);
                if (paragraph == null)
                {
                    return Error<Paragraph>("Paragraph not found");
                }

                paragraph.IsLocked = false;
                paragraph.LockedBy = null;
                paragraph.LockedAt = null;
                paragraph.UpdatedAt = DateTime.UtcNow;

                _context.Paragraphs.Update(paragraph);
                await _context.SaveChangesAsync();

                return Success(paragraph, "Paragraph unlocked successfully");
            }
            catch (Exception ex)
            {
                return Error<Paragraph>($"Error unlocking paragraph: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Translation>>> GetParagraphTranslations(int paragraphId)
        {
            try
            {
                var translations = await _context.Translations
                    .Where(t => t.ParagraphId == paragraphId)
                    .Include(t => t.Translator)
                    .Include(t => t.Reviews)
                    .OrderByDescending(t => t.Version)
                    .ToListAsync();

                return Success(translations);
            }
            catch (Exception ex)
            {
                return Error<List<Translation>>($"Error retrieving translations: {ex.Message}");
            }
        }

        public async Task<ServiceResult<List<Paragraph>>> CreateParagraphs(List<Paragraph> paragraphs)
        {
            try
            {
                _context.Paragraphs.AddRange(paragraphs);
                await _context.SaveChangesAsync();

                return Success(paragraphs, "Paragraphs created successfully");
            }
            catch (Exception ex)
            {
                return Error<List<Paragraph>>($"Error creating paragraphs: {ex.Message}");
            }
        }
    }
}