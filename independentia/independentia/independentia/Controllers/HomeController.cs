using System.Diagnostics;
using independentia.Data;
using Microsoft.AspNetCore.Mvc;
using independentia.Models;
using independentia.Views.Category;
using SQLitePCL;

namespace independentia.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
   

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;

    }

    public IActionResult Index()
    {
        HomePageModel model=new HomePageModel();
        model.MostPurchased = _context.Films
            .OrderByDescending(f => f.CreatedDate)
            .Take(5)
            .ToList();
        model.MostWatched = _context.Films
            .OrderByDescending(f => f.WatchCount)
            .Take(5)
            .ToList();
        return View(model);
    }

    

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Films()
    {
        return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}