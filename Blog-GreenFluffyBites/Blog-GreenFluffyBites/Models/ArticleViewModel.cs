using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Blog_GreenFluffyBites.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55)]
        public string Title { get; set; }

        [Required]
        [DisplayName("Article")]
        public string Content { get; set; }

        [ForeignKey("Category")]
        [DisplayName("Category")]
        [Required]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

        public string AuthorId { get; set; }

        public int Score { get; set; }
    }
}