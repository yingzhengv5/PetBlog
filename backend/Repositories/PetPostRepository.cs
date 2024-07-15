using Microsoft.EntityFrameworkCore;
using PetBlog.Data;
using PetBlog.Models;

namespace PetBlog.Repositories
{
    public class PetPostRepository : IPetPostRepository
    {
        private readonly PetBlogContext _context;

        public PetPostRepository(PetBlogContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PetPost>> GetAllPetPostsAsync()
        {
            return await _context.PetPosts.ToListAsync();
        }

        public async Task<PetPost> GetPetPostByIdAsync(long id)
        {
            return await _context.PetPosts.FindAsync(id);
        }

        public async Task AddPetPostAsync(PetPost petPost)
        {
            await _context.PetPosts.AddAsync(petPost);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePetPostAsync(PetPost petPost)
        {
            _context.Entry(petPost).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePetPostAsync(long id)
        {
            var petPost = await _context.PetPosts.FindAsync(id);
            if (petPost != null)
            {
                _context.PetPosts.Remove(petPost);
                await _context.SaveChangesAsync();
            }
        }
    }
}