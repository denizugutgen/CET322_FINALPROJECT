namespace independentia.Models;

public class Film
{
    public int ID { get; set; } 
    public string? PosterUrl { get; set; }
    
    public string? TrailerUrl { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public string Director { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    public float Price { get; set; }
    public int? CategoryID { get; set; }
    public virtual Category? Category { get; set; }
    
    public int? WatchCount { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    
    public float Rating { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    
}