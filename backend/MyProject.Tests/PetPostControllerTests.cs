using Moq;
using Microsoft.AspNetCore.Mvc;
using PetBlog.Controllers;
using PetBlog.Repositories;
using CloudinaryDotNet;
using PetBlog.Models;
using Microsoft.Extensions.Logging;

namespace PetBlog.Tests
{
    public class PetPostControllerTests
    {
        private readonly Mock<IPetPostRepository> _mockRepository;
        private readonly Mock<ILogger<PetPostController>> _mockLogger;
        private readonly Cloudinary _cloudinary;
        private readonly PetPostController _controller;

        public PetPostControllerTests()
        {
            _mockRepository = new Mock<IPetPostRepository>();
            _mockLogger = new Mock<ILogger<PetPostController>>();

            var cloudinaryAccount = new Account("dummycloud", "dummykey", "dummysecret");
            _cloudinary = new Cloudinary(cloudinaryAccount);

            _controller = new PetPostController(_mockRepository.Object, _mockLogger.Object, _cloudinary);
        }

        [Fact]
        public async Task GetPetPosts_ReturnsOkResult_WithListOfPetPosts()
        {
            // Arrange
            var mockPetPosts = new List<PetPost>
            {
                new PetPost { Id = 1, Title = "Post 1", Content = "Content for post 1" },
                new PetPost { Id = 2, Title = "Post 2", Content = "Content for post 2" }
            };
            _mockRepository.Setup(repo => repo.GetAllPetPostsAsync()).ReturnsAsync(mockPetPosts);

            // Act
            var result = await _controller.GetPetPosts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PetPost>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetPetPost_ReturnsOkResult_WithPetPost()
        {
            // Arrange
            var mockPetPost = new PetPost { Id = 1, Title = "Post 1", Content = "Content for post 1" };
            _mockRepository.Setup(repo => repo.GetPetPostByIdAsync(1)).ReturnsAsync(mockPetPost);

            // Act
            var result = await _controller.GetPetPost(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PetPost>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Post 1", returnValue.Title);
        }

        [Fact]
        public async Task GetPetPost_ReturnsNotFound_WhenPetPostNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetPetPostByIdAsync(1)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.GetPetPost(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Post with not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task AddPetPost_ReturnsCreatedAtActionResult_WithPetPost()
        {
            // Arrange
            var newPetPost = new PetPost { Id = 1, Title = "New Post", Content = "Content for new post" };
            _mockRepository.Setup(repo => repo.AddPetPostAsync(newPetPost)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddPetPost(newPetPost, null);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<PetPost>(createdResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("New Post", returnValue.Title);
        }

        [Fact]
        public async Task UpdatePetPost_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var updatedPetPost = new PetPost { Id = 1, Title = "Updated Post", Content = "Updated content" };
            _mockRepository.Setup(repo => repo.UpdatePetPostAsync(updatedPetPost)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdatePetPost(1, updatedPetPost, null, null, null);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeletePetPost_ReturnsNotFound_WhenPetPostNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.DeletePetPostAsync(1)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeletePetPost(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
