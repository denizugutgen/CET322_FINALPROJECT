using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using independentia.Data;
using independentia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace independentia.Controllers;

public class CustomerController: Controller
{
    private readonly ApplicationDbContext _context;
    
    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        return View(await _context.Customers.ToListAsync());
    }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,Name,Surname,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,Name,Surname,Email")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            return View(customer);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }

        

        [Authorize]
        public async Task<IActionResult> MyArchive()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var purchasedFilms = await _context.FilmPurchases
                .Include(f => f.Film)
                .Where(f => f.UserId == UserId)
                .ToListAsync();
            return View(purchasedFilms);
            
        }

        [Authorize]
        public async Task<IActionResult> AddFakePurchase(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var purchase = new FilmPurchase
            {
                UserId = userId,
                FilmID = filmId,
                PurchaseDate = DateTime.Now,
            };
            _context.FilmPurchases.Add(purchase);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyArchive");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Buy(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alreadyPurchased = await _context.FilmPurchases
                .AnyAsync(f => f.UserId == userId && f.FilmID == filmId);
            if (alreadyPurchased)
            {
                TempData["Message"] = "You have already purchased this film.";
                return RedirectToAction("Details", "Film", new { id = filmId });
            }

            var purchase = new FilmPurchase
            {
                UserId = userId,
                FilmID = filmId,
                PurchaseDate = DateTime.Now,
            };
            _context.FilmPurchases.Add(purchase);
            await _context.SaveChangesAsync();
            
            TempData["Message"] = "Purchase successful!";
            return RedirectToAction("MyArchive");
        }
        [HttpPost]
        [Authorize]
        public IActionResult Checkout(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var film = _context.Films.FirstOrDefault(f => f.ID == filmId);

            if (film == null) return NotFound();
            
            var alreadyPurchased = _context.FilmPurchases
                .Any(p => p.UserId == userId && p.FilmID == filmId);

            if (alreadyPurchased)
            {
                TempData["Message"] = "You have already purchased this film.";
                return RedirectToAction("MyArchive");
            }

            return View("Checkout", film); 
        }
        

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckoutConfirmed(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var alreadyPurchased = await _context.FilmPurchases
                .AnyAsync(f => f.UserId == userId && f.FilmID == filmId);
            if(!alreadyPurchased)
            {
                var purchase = new FilmPurchase
                {
                    UserId = userId,
                    FilmID = filmId,
                    PurchaseDate = DateTime.Now,
                };
                _context.FilmPurchases.Add(purchase);
                await _context.SaveChangesAsync();
                
                TempData["Message"] = "Checkout successful!";
            }

            return RedirectToAction("MyArchive");

        }
        [HttpPost]
        [Authorize]
        public IActionResult ConfirmPurchase(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var film = _context.Films.FirstOrDefault(f => f.ID == filmId);

            if (film ==null)
            {
                return NotFound();
            }
            var alreadyPurchased = _context.FilmPurchases
                .Any(p => p.UserId == userId && p.FilmID == filmId);
            if (alreadyPurchased)
            {
                TempData["Message"] = "You have already purchased this film.";
                return RedirectToAction("MyArchive");
            }

            var purchase = new FilmPurchase()
            {
                FilmID = filmId,
                UserId = userId,
                PurchaseDate = DateTime.Now
            };
            _context.FilmPurchases.Add(purchase);
            _context.SaveChanges();
            TempData["Message"] = "Purchase confirmed!";
            return RedirectToAction("MyArchive");
        }
        
}