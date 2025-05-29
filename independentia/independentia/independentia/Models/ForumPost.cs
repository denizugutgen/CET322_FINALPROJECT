namespace independentia.Models;

public class ForumPost
{
    public int ID { get; set; }
    public string Content { get; set; }
    public string UserId { get; set; }
    public DateTime PostedAt { get; set; }
    
    public int TopicID { get; set; }
    public virtual ForumTopic Topic { get; set; }
    
}