using System.ComponentModel.DataAnnotations;

namespace independentia.Models;

public class Category
{
    [Key]
    
    public int ID { get; set; }
    [MaxLength(100)]
    [Required]
    
    public string Name { get; set; }
    
    public virtual List<Film>? Films { get; set; }
}