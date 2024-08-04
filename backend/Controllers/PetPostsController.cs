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
        private readonly ILogger<PetPostController> _logger;
        private readonly Cloudinary _cloudinary;
        private readonly string _defaultImageUrl = "https://images.unsplash.com/photo-1554456854-55a089fd4cb2?q=80&w=2670&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D";

        public PetPostController(IPetPostRepository repository, ILogger<PetPostController> logger, Cloudinary cloudinary)
        {
            _repository = repository;
            _logger = logger;
            _cloudinary = cloudinary;
        }

        // GET: api/PetPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetPost>>> GetPetPosts()
        {
            try
            {
                var petPosts = await _repository.GetAllPetPostsAsync();
                return Ok(petPosts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetPetPosts");
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        //Get: api/PetPosts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<PetPost>> GetPetPost(long id)
        {
            try
            {
                var petPost = await _repository.GetPetPostByIdAsync(id);
                return Ok(petPost);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Post with not found.");
            }
        }

        //Post: api/PetPosts
        [HttpPost]
        public async Task<ActionResult<PetPost>> AddPetPost([FromForm] PetPost petPost, [FromForm] IFormFile[]? images)
        {
            try
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

                    petPost.ImageUrls = imageUrls;
                }
                else
                {
                    petPost.ImageUrls = new List<string> { _defaultImageUrl }; // Default image
                }

                await _repository.AddPetPostAsync(petPost);
                return CreatedAtAction(nameof(GetPetPost), new { id = petPost.Id }, petPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in AddPetPost");
                return Ok("An error occurred while processing your request.");
            }
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
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Post with not found.");
            }
        }

        //Delete: api/PetPosts/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetPost(long id)
        {
            try
            {
                var petPost = await _repository.GetPetPostByIdAsync(id);
                if (petPost == null)
                {
                    return NotFound();
                }

                await _repository.DeletePetPostAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}