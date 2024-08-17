using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Data;
using PhotoGallery.Models;

namespace PhotoGallery.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Comments/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Comment comment)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"Received comment: Author={comment.Author}, Text={comment.Text}, PhotoId={comment.PhotoId}");
                comment.DatePosted = DateTime.UtcNow;
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return Ok();
            }

            var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.Errors = errorList;
            Console.WriteLine($"Validation errors: {string.Join(", ", errorList)}");
            return BadRequest(new { errors = errorList });
        }

        // GET: Comments/GetCommentsByPhotoId
        [HttpGet("GetCommentsByPhotoId")]
        public async Task<IActionResult> GetCommentsByPhotoId(int photoId)
        {

           
            {
                var comments = await _context.Comments
                    .Where(c => c.PhotoId == photoId)
                    .OrderByDescending(c => c.DatePosted)
                    .ToListAsync();

                if (comments == null || !comments.Any())
                {
                    return NoContent();
                }
                return Json(comments);
            }

            
        }
        // GET: Comments/PhotoWithComments
        [HttpGet("PhotoWithComments/{photoId}")]
        public async Task<IActionResult> PhotoWithComments(int photoId)
        {
            var photo = await _context.Photos.FindAsync(photoId);
            if (photo == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Where(c => c.PhotoId == photoId)
                .OrderByDescending(c => c.DatePosted)
                .ToListAsync();

            var viewModel = new PhotoWithCommentsViewModel
            {
                Photo = photo,
                Comments = comments
            };

            return View(viewModel);
        }
    }

    public class PhotoWithCommentsViewModel
    {
        public Photo? Photo { get; set; }
        public List<Comment> Comments { get; set; }
    }

}
