using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        private readonly Cloudinary _cloudinary;
        private readonly string _defaultImageUrl = "http://res.cloudinary.com/dev7ehsz4/image/upload/v1721350123/ghbgjup4fh4lhvibfdox.png";

        public PetPostController(IPetPostRepository repository, Cloudinary cloudinary)
        {
            _repository = repository;
            _cloudinary = cloudinary;
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
        public async Task<ActionResult<PetPost>> AddPetPost([FromForm] PetPost petPost, [FromForm] IFormFile[]? images)
        {
            var imageUrls = new List<string>();

            if (images != null && images.Length > 0)
            {
                foreach (var image in images)
                {
                    var uploadResult = new ImageUploadResult();
                    using (var stream = image.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(image.FileName, stream)
                        };
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        imageUrls.Add(uploadResult.SecureUrl.AbsoluteUri);
                    }
                }

                petPost.ImageUrls = imageUrls; // Assuming you have updated your model to handle multiple URLs
            }
            else
            {
                petPost.ImageUrls = new List<string> { _defaultImageUrl }; // Default image
            }

            await _repository.AddPetPostAsync(petPost);
            return CreatedAtAction(nameof(GetPetPost), new { id = petPost.Id }, petPost);
        }

        //Put: api/PetPosts/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePetPost(long id, [FromForm] PetPost petPost, [FromForm] IFormFile[]? images, [FromForm] string[] existingImages, [FromForm] string[] deletedImages)
        {
            if (id != petPost.Id)
            {
                return BadRequest();
            }

            try
            {
                // Combine existing images and new images
                var imageUrls = new List<string>();

                if (existingImages != null && existingImages.Length > 0)
                {
                    imageUrls.AddRange(existingImages);
                }

                if (images != null && images.Length > 0)
                {
                    foreach (var image in images)
                    {
                        var uploadResult = new ImageUploadResult();
                        using (var stream = image.OpenReadStream())
                        {
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(image.FileName, stream)
                            };
                            uploadResult = await _cloudinary.UploadAsync(uploadParams);
                            imageUrls.Add(uploadResult.SecureUrl.AbsoluteUri);
                        }
                    }
                }

                // Handle deleted images
                if (deletedImages != null && deletedImages.Length > 0)
                {
                    foreach (var imageUrl in deletedImages)
                    {
                        // Extract public ID from URL
                        var publicId = imageUrl.Split('/').Last().Split('.').First();
                        await _cloudinary.DestroyAsync(new DeletionParams(publicId));
                    }

                    // Remove deleted images from the combined list
                    imageUrls = imageUrls.Except(deletedImages).ToList();
                }

                // Check if no images are left and set default image
                if (!imageUrls.Any())
                {
                    imageUrls.Add(_defaultImageUrl);
                }

                petPost.ImageUrls = imageUrls.Count > 0 ? imageUrls : null;

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