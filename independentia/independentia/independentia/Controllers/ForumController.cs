using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using independentia.Data;
using independentia.Models;

namespace independentia.Controllers
{
    [Authorize]
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            var topics = await _context.ForumTopics
                .Include(t => t.Posts)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(topics);
        }

        

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                ModelState.AddModelError("Title", "Title cannot be empty.");
                return View();
            }

            var topic = new ForumTopic
            {
                Title = title,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedAt = DateTime.Now
            };

            _context.ForumTopics.Add(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var topic = await _context.ForumTopics
                .Include(t => t.Posts)
                .FirstOrDefaultAsync(t => t.ID == id);

            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostReply(int topicId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "Reply content cannot be empty.";
                return RedirectToAction("Details", new { id = topicId });
            }

            var post = new ForumPost
            {
                TopicID = topicId,
                Content = content,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                PostedAt = DateTime.Now
            };

            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = topicId });
        }
        
    }
}
