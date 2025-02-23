namespace BookManagement.Data.Entities;

public class BookEntity : Entity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int ViewsCount { get; set; }
    public int PublicationYear { get; set; }
}