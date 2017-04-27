using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog_GreenFluffyBites.Models
{
    public class Profile
    {
        [Required]
        [Display(Name = "Change your avatar:")]
        public byte[] ProfilePicture { get; set; }
    }
}