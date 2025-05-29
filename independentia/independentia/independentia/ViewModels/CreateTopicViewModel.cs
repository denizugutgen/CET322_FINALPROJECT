using System.ComponentModel.DataAnnotations;

namespace independentia.ViewModels
{
    public class CreateTopicViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}