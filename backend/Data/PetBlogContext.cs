using Microsoft.EntityFrameworkCore;
using PetBlog.Models;

namespace PetBlog.Data
{
    public class PetBlogContext : DbContext
    {
        public PetBlogContext(DbContextOptions<PetBlogContext> options)
            : base(options)
        {
        }

        public DbSet<PetPost> PetPosts { get; set; }
    }
}