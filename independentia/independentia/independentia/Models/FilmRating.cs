using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace independentia.Models
{
    public class FilmRating
    {
        public int ID { get; set; }
        [Required]
        public int FilmID { get; set; }
        public Film? Film { get; set; }
        [Required]
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        
    
    }
}

