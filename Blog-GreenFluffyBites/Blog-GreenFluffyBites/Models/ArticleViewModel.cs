using System.ComponentModel.DataAnnotations;


namespace Blog_GreenFluffyBites.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55)]
        public string Title { get; set; }
        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        public string AuthorId { get; set; }
    }
}