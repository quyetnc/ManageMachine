using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ManageMachine.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public SettingsController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("login-background")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLoginBackground(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Validate file type (optional but good practice)
            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("File must be an image.");

            // Define path: wwwroot/assets/images/login-bg.png
            var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(webRootPath, "assets", "images");

            // Ensure directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, "login-bg.png");

            // Overwrite existing file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "Login background updated successfully." });
        }
    }
}
