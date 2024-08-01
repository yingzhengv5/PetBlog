using Microsoft.EntityFrameworkCore;
using PetBlog.Data;
using PetBlog.Models;
using Microsoft.Extensions.Logging;

namespace PetBlog.Repositories
{
    public class PetPostRepository : IPetPostRepository
    {
        private readonly PetBlogContext _context;
        private readonly ILogger<PetPostRepository> _logger;

        public PetPostRepository(PetBlogContext context, ILogger<PetPostRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PetPost>> GetAllPetPostsAsync()
        {
            try
            {
                // Retrieve all pet posts from the database
                return await _context.PetPosts.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred.");
                throw;
            }
        }

        public async Task<PetPost> GetPetPostByIdAsync(long id)
        {
            try
            {
                // Find a pet post by ID
                var petPost = await _context.PetPosts.FindAsync(id);
                if (petPost == null)
                {
                    throw new KeyNotFoundException("Post not found.");
                }
                return petPost;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred.");
                throw;
            }
        }

        public async Task AddPetPostAsync(PetPost petPost)
        {
            if (petPost == null)
            {
                throw new ArgumentNullException(nameof(petPost));
            }

            try
            {
                // Add the new pet post to the context and save changes
                await _context.PetPosts.AddAsync(petPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred.");
                throw;
            }
        }

        public async Task UpdatePetPostAsync(PetPost petPost)
        {
            if (petPost == null)
            {
                throw new ArgumentNullException(nameof(petPost));
            }

            try
            {
                // Mark the entity as modified and save changes
                _context.Entry(petPost).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred.");
                throw;
            }
        }

        public async Task DeletePetPostAsync(long id)
        {

            try
            {
                // Find the pet post by ID
                var petPost = await _context.PetPosts.FindAsync(id);
                if (petPost == null)
                {
                    throw new KeyNotFoundException($"PetPost with id {id} not found.");
                }

                // Remove the pet post from the context and save changes
                _context.PetPosts.Remove(petPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred.");
                throw;
            }
        }
    }
}