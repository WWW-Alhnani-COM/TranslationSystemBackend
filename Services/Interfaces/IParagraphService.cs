using TranslationDB.Models;
using TranslationDB.Models.DTOs;

namespace TranslationDB.Services.Interfaces
{
    public interface IParagraphService
    {
        Task<ServiceResult<List<Paragraph>>> GetAllParagraphs();
        Task<ServiceResult<Paragraph>> GetParagraphById(int id);
        Task<ServiceResult<Paragraph>> UpdateParagraph(Paragraph paragraph);
        Task<ServiceResult<Paragraph>> LockParagraph(int paragraphId, string userId);
        Task<ServiceResult<Paragraph>> UnlockParagraph(int paragraphId);
        Task<ServiceResult<List<Translation>>> GetParagraphTranslations(int paragraphId);
        Task<ServiceResult<List<Paragraph>>> CreateParagraphs(List<Paragraph> paragraphs);
    }
}