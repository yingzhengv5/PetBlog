using PetBlog.Models;

namespace PetBlog.Repositories
{
    public interface IPetPostRepository
    {
        Task<IEnumerable<PetPost>> GetAllPetPostsAsync();
        Task<PetPost> GetPetPostByIdAsync(long id);
        Task AddPetPostAsync(PetPost petPost);
        Task UpdatePetPostAsync(PetPost petPost);
        Task DeletePetPostAsync(long id);
    }
}