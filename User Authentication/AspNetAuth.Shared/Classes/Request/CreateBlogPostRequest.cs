using System.ComponentModel.DataAnnotations;

namespace AspNetAuth.Shared.Classes.Request
{
    public class CreateBlogPostRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }
    }
}