namespace BookManagement.Core.Models;

public class BookModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
}

public class DetailBookModel : BookModel
{
    public double PopularityScore { get; set; }
}