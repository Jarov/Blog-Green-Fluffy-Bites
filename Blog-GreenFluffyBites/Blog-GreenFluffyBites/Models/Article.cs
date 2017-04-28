using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Blog_GreenFluffyBites.Models
{
    public class Article
    {
        private ICollection<Comment> comments;

        public Article()
        {
            DatePosted = DateTime.Now;
            UsersLikesIDs = string.Empty;
            Comments = new HashSet<Comment>();
        }
        public Article(string authorID, string title, string content, int categoryId)
        {
            this.AuthorId = authorID;
            this.Title = title;
            this.Content = content;
            this.comments = new HashSet<Comment>();
            this.CategoryId = categoryId;
            UsersLikesIDs = String.Empty;
        }

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

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }
    }
}