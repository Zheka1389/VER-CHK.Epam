using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VER_CHK.Models.Articles
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string CreatedUser { get; set; }

        [Required]
        public string CreatedDate { get; set; }

        public List<CommentModel> Comment { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
    }
}
