using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using independentia.Data;
using independentia.Models;
using Microsoft.AspNetCore.Authorization;

namespace independentia.Controllers;

public class FilmController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public FilmController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var films = await _context.Films
            .Include(f => f.Category)
            .ToListAsync();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var purchasedIds = new List<int>();

        if (!string.IsNullOrEmpty(userId))
        {
            purchasedIds = await _context.FilmPurchases
                .Where(fp => fp.UserId == userId)
                .Select(fp => fp.FilmID)
                .ToListAsync();
        }
        ViewBag.PurchasedFilmIds = purchasedIds;

        return View(films);
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var film = await _context.Films
            .Include(f => f.Category)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (film == null)
        {
            return NotFound();
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var purchasedIds = new List<int>();
        var rating = await _context.FilmRatings
            .FirstOrDefaultAsync(r => r.FilmID == id && r.UserId == userId);
        ViewBag.UserRating = rating?.Rating ?? 0;
        if ((int)ViewBag.UserRating > 0)
        {
            ViewBag.UserRating = rating.Rating;
        }
        else
        {
            ViewBag.UserRating = 0;
        }

        if (!string.IsNullOrEmpty(userId))
        {
            purchasedIds = await _context.FilmPurchases
                .Where(fp => fp.UserId == userId)
                .Select(fp => fp.FilmID)
                .ToListAsync();
        }
        ViewBag.PurchasedFilmIds = purchasedIds;

        return View(film);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name");

        return View();
    }    
    [HttpPost]
    [ValidateAntiForgeryToken]
    
    public async Task<IActionResult> Create([Bind("ID,Title,Description,Director,Price,CategoryID,PosterUrl")] Film film)
    {
        if (ModelState.IsValid)
        {
            _context.Add(film);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", film.CategoryID);
        return View(film);
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var film = await _context.Films.FindAsync(id);
        if (film == null)
        {
            return NotFound();
        }
        ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", film.CategoryID);
        return View(film);
    }   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,Director,PosterUrl,Price,CategoryID")] Film film)
    {
        if (id != film.ID)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(film);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(film.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", film.CategoryID);
        return View(film);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var film = await _context.Films
            .Include(f => f.Category)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (film == null)
        {
            return NotFound();
        }
        return View(film);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film != null)
        {
            _context.Films.Remove(film);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Rate(int filmId, int rating)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Önce kullanıcının bu filmi satın alıp almadığını kontrol et
        var hasPurchased = await _context.FilmPurchases
            .AnyAsync(fp => fp.UserId == userId && fp.FilmID == filmId);

        if (!hasPurchased)
        {
            TempData["ErrorMessage"] = "You must purchase the film before rating it.";
            return RedirectToAction("Details", new { id = filmId });
        }

        // Daha önce rating verdiyse güncelle, yoksa yeni ekle
        var existingRating = await _context.FilmRatings
            .FirstOrDefaultAsync(r => r.FilmID == filmId && r.UserId == userId);

        if (existingRating != null)
        {
            existingRating.Rating = rating;
            _context.Update(existingRating);
        }
        else
        {
            var filmRating = new FilmRating
            {
                FilmID = filmId,
                UserId = userId,
                Rating = rating
            };
            _context.FilmRatings.Add(filmRating);
        }

        // Kaydet
        await _context.SaveChangesAsync();

        // Ortalama puanı güncelle
        var allRatings = await _context.FilmRatings
            .Where(r => r.FilmID == filmId)
            .Select(r => r.Rating)
            .ToListAsync();

        var film = await _context.Films.FindAsync(filmId);
        film.Rating = (float)allRatings.Average();
        _context.Update(film);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thank you for rating the film!";
        return RedirectToAction("Details", new { id = filmId });
    }
    
    private bool FilmExists(int id)
    {
        return _context.Films.Any(e => e.ID == id);
    }
    
    
}