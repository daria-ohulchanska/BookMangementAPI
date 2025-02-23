using BookManagement.Core.Models;
using BookManagement.Core.Services;
using BookManagement.Data.Entities;
using BookManagement.Data.Repositories;
using Moq;

namespace BookManagement.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _bookService = new BookService(_mockBookRepository.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var bookId = 1;
            var book = new BookEntity { Id = bookId, Title = "Test Book", Author = "Test Author" };
            _mockBookRepository.Setup(repo => repo.GetAsync(bookId)).ReturnsAsync(book);

            // Act
            var result = await _bookService.GetAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
        }

        [Fact]
        public async Task GetAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = 1;
            _mockBookRepository.Setup(repo => repo.GetAsync(bookId)).ReturnsAsync((BookEntity?)null);

            // Act
            var result = await _bookService.GetAsync(bookId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsBookSuccessfully()
        {
            // Arrange
            var bookEntity = new BookEntity { Id = 1, Title = "New Book", Author = "New Author", PublicationYear = 2023 };
            var bookModel = new BookModel { Title = bookEntity.Title, Author = bookEntity.Author, PublicationYear = bookEntity.PublicationYear };
            _mockBookRepository.Setup(repo => repo.AddAsync(It.IsAny<BookEntity>()));

            // Act
            var result = await _bookService.AddAsync(bookModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookEntity.Title, result.Title);
            _mockBookRepository.Verify(repo => repo.AddAsync(It.IsAny<BookEntity>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesBookSuccessfully()
        {
            // Arrange
            var bookEntity = new BookEntity { Id = 1, Title = "Updated Book", Author = "Updated Author", PublicationYear = 2023 };
            var bookModel = new BookModel { Id = 1, Title = "Updated Book", Author = "Updated Author", PublicationYear = 2023 };
            _mockBookRepository.Setup(repo => repo.UpdateAsync(bookEntity)).Returns(Task.CompletedTask);

            // Act
            await _bookService.UpdateAsync(bookModel);

            // Assert
            _mockBookRepository.Verify(repo => repo.UpdateAsync(It.IsAny<BookEntity>()), Times.Once);
        }

        [Fact]
        public async Task SoftDeleteAsync_DeletesBookSuccessfully()
        {
            // Arrange
            var bookId = 1;
            _mockBookRepository.Setup(repo => repo.SoftDeleteAsync(bookId)).Returns(Task.CompletedTask);

            // Act
            await _bookService.SoftDeleteAsync(bookId);

            // Assert
            _mockBookRepository.Verify(repo => repo.SoftDeleteAsync(bookId), Times.Once);
        }
    }
}