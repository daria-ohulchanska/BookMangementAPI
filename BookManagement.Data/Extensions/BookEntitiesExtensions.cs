using BookManagement.Data.Entities;

namespace BookManagement.Data.Extensions;

public static class BookEntitiesExtensions
{
    public static double CalculatePopularity(this BookEntity book)
    {
        return (book.ViewsCount * 0.5) + ((DateTime.Now.Year - book.PublicationYear) * 2);
    }
}