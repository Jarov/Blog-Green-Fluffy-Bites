using System.ComponentModel.DataAnnotations;


namespace Blog_GreenFluffyBites.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public string content { get; set; }

        public int Score { get; set; }
        
        public bool IsAuthor(string name)
        {
            return this.AuthorName.Equals(name);
        }
    }
}
