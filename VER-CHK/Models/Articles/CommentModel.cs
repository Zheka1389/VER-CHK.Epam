using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VER_CHK.Models.Articles
{
    public class CommentModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string CreatedUser { get; set; }

        [Required]
        public string CreatedDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Comment { get; set; }
    }
}
