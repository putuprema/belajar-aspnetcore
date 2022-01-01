using System.ComponentModel.DataAnnotations;

namespace AspNetAuth.WebApp.ViewModels
{
    public class NewBlogViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }
    }
}