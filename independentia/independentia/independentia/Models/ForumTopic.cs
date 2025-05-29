namespace independentia.Models;

public class ForumTopic
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Content { get; set; }
    
    public virtual List<ForumPost> Posts { get; set; }
    
}