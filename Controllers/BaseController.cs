using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TranslationDB.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(T result, string message = "")
        {
            if (result == null)
                return NotFound(new { success = false, message = "Record not found" });

            return Ok(new { success = true, data = result, message });
        }

        protected IActionResult HandleError(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, new { success = false, message });
        }
    }
}