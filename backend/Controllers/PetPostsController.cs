using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetBlog.Models;
using PetBlog.Repositories;

namespace PetBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetPostController : ControllerBase
    {
        private readonly IPetPostRepository _repository;

        public PetPostController(IPetPostRepository repository)
        {
            _repository = repository;
        }

        // GET: api/PetPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetPost>>> GetPetPosts()
        {
            var petPosts = await _repository.GetAllPetPostsAsync();
            return Ok(petPosts);
        }

        //Get: api/PetPosts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<PetPost>> GetPetPost(long id)
        {
            var petPost = await _repository.GetPetPostByIdAsync(id);

            if (petPost != null)
            {
                return Ok(petPost);
            }
            return NotFound();
        }

        //Post: api/PetPosts
        [HttpPost]
        public async Task<ActionResult<PetPost>> AddPetPost(PetPost petPost)
        {
            await _repository.AddPetPostAsync(petPost);
            return CreatedAtAction(nameof(GetPetPost), new { id = petPost.Id }, petPost);
        }

        //Put: api/PetPosts/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePetPost(long id, PetPost petPost)
        {
            if (id != petPost.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repository.UpdatePetPostAsync(petPost);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _repository.GetPetPostByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //Delete: api/PetPosts/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetPost(long id)
        {
            var petPost = await _repository.GetPetPostByIdAsync(id);
            if (petPost == null)
            {
                return NotFound();
            }

            await _repository.DeletePetPostAsync(id);
            return NoContent();
        }
    }
}