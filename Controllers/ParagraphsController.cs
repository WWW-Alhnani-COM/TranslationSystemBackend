using Microsoft.AspNetCore.Mvc;
using TranslationDB.Models;
using TranslationDB.Models.DTOs;
using TranslationDB.Services;
using TranslationDB.Services.Interfaces;

namespace TranslationDB.Controllers
{
    public class ParagraphsController : BaseController
    {
        private readonly IParagraphService _paragraphService;

        public ParagraphsController(IParagraphService paragraphService)
        {
            _paragraphService = paragraphService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllParagraphs()
        {
            try
            {
                var paragraphs = await _paragraphService.GetAllParagraphs();
                return HandleResult(paragraphs);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving paragraphs: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetParagraphById(int id)
        {
            try
            {
                var paragraph = await _paragraphService.GetParagraphById(id);
                return HandleResult(paragraph);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving paragraph: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParagraph(int id, Paragraph paragraph)
        {
            try
            {
                if (id != paragraph.Id)
                    return HandleError("ID mismatch");

                var result = await _paragraphService.UpdateParagraph(paragraph);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Paragraph updated successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error updating paragraph: {ex.Message}");
            }
        }

        [HttpPost("{id}/lock")]
        public async Task<IActionResult> LockParagraph(int id, [FromBody] LockParagraphDto lockDto)
        {
            try
            {
                var result = await _paragraphService.LockParagraph(id, lockDto.UserId);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Paragraph locked successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error locking paragraph: {ex.Message}");
            }
        }

        [HttpPost("{id}/unlock")]
        public async Task<IActionResult> UnlockParagraph(int id)
        {
            try
            {
                var result = await _paragraphService.UnlockParagraph(id);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Paragraph unlocked successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error unlocking paragraph: {ex.Message}");
            }
        }

        [HttpGet("{id}/translations")]
        public async Task<IActionResult> GetParagraphTranslations(int id)
        {
            try
            {
                var translations = await _paragraphService.GetParagraphTranslations(id);
                return HandleResult(translations);
            }
            catch (Exception ex)
            {
                return HandleError($"Error retrieving translations: {ex.Message}");
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateParagraphs([FromBody] List<Paragraph> paragraphs)
        {
            try
            {
                var result = await _paragraphService.CreateParagraphs(paragraphs);
                if (!result.Success)
                    return HandleError(result.Message);

                return HandleResult(result.Data, "Paragraphs created successfully");
            }
            catch (Exception ex)
            {
                return HandleError($"Error creating paragraphs: {ex.Message}");
            }
        }
    }
}