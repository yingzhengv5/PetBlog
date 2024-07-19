using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetBlog.Models
{
    public class PetPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public required string Title { get; set; }
        public required string Content { get; set; }
        public List<string>? ImageUrls { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
