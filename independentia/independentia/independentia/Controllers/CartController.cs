namespace independentia.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using independentia.Data;
using independentia.Models;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cartItems = await _context.CartItems
            .Include(ci => ci.Film)
            .Where(ci => ci.UserId == userId)
            .ToListAsync();
        return View(cartItems);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int filmId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var alreadyInCart = await _context.CartItems
            .AnyAsync(ci => ci.FilmID == filmId && ci.UserId == userId);
        if (!alreadyInCart)
        {
            var cartItem = new CartItem
            {
                FilmID = filmId,
                UserId = userId
            };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(int cartItemId)
    {
        var item = await _context.CartItems.FindAsync(cartItemId);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");

    }
}