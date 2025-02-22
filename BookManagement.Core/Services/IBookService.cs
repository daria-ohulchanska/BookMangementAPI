using BookManagement.Core.Models;
using BookManagement.Shared.Models;

namespace BookManagement.Core.Services;

public interface IBookService
{
    Task<DetailBookModel?> GetAsync(int id);
    Task<List<string>> GetTitlePageAsync(PaginationParams paginationParams);
    Task<BookModel> AddAsync(BookModel book);
    Task<List<BookModel>> AddRangeAsync(List<BookModel> books);
    Task UpdateAsync(BookModel book);
    Task SoftDeleteAsync(int id);
    Task SoftDeleteAsync(List<int> ids);
}