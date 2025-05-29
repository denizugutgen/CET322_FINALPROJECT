using System;
using System.ComponentModel.DataAnnotations;
using independentia.Controllers;
using Microsoft.AspNetCore.Identity;

namespace independentia.Models;

public class FilmPurchase
{
    public int ID { get; set; }
    
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public int FilmID { get; set; }
    public Film Film { get; set; }
    public string? Description { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.Now;
}