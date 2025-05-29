namespace independentia.Models;

public class CartItem
{
    public int ID { get; set; }
    public int FilmID { get; set; }
    public Film Film { get; set; }
    public string UserId { get; set; }
}