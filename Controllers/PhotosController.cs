using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Data;
using PhotoGallery.Models;
using System;


namespace PhotoGallery.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhotosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Photos.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                // Ensure the uploads directory exists
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                var filePath = Path.Combine(uploadsDirectory, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var userName = User.Identity.Name ?? "Unknown"; // Если имя пользователя не найдено, устанавливаем "Unknown"

                var photo = new Photo
                {
                    Url = "/uploads/" + file.FileName,
                    FilePath = file.FileName,
                    UploadedBy = userName,
                    UploadDate = DateTime.Now,
                    Description = "Some description", // Установите соответствующее описание
                    UploadedAt = DateTime.Now
                };

                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View("Index");
        }
    }
}
