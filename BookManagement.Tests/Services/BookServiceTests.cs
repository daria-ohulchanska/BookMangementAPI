using Xunit;

public class BookServiceTests
{
    [Fact]
    public void Test_AddBook_ShouldReturnTrue_WhenBookIsValid()
    {
        // Arrange
        var bookService = new BookService();
        var book = new Book { Title = "Test Book", Author = "Author" };

        // Act
        var result = bookService.AddBook(book);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Test_GetBook_ShouldReturnBook_WhenBookExists()
    {
        // Arrange
        var bookService = new BookService();
        var book = new Book { Title = "Test Book", Author = "Author" };
        bookService.AddBook(book);

        // Act
        var result = bookService.GetBook(book.Title);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(book.Title, result.Title);
    }
}