using System;
using System.ComponentModel.DataAnnotations;

namespace Blog_GreenFluffyBites.Models
{
    public class Comment
    {
        public Comment()
        {
            UsersLikesIDs = String.Empty;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string content { get; set; }

        public virtual Article Article { get; set; }

        public int ArticleId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string AuthorId { get; set; }
        
        public int Score { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UsersLikesIDs { get; set; }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}