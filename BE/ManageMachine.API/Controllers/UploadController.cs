using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ManageMachine.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var folderName = Path.Combine("wwwroot", "uploads");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine("uploads", fileName).Replace("\\", "/");

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path or full URL. Relative path is flexible.
            // Client can prepend base URL.
            // Or return full URL if we know the host.
            // Let's return the relative path (e.g. "uploads/abc.jpg") and let client/static files handle it.
            // Actually, for easy access, let's return a helper object
            
            return Ok(new { url = dbPath });
        }
    }
}
