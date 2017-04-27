using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Blog_GreenFluffyBites.Models
{
    public class Article
    {
        private ICollection<Comment> comments;

        [Key]
        public int Id { get; set; }

        [Required]        
        [StringLength(55)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int Score { get; set; }

        public string ImagePath { get; set; }

        public DateTime DatePosted { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UsersLikesIDs { get; set; }
        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}