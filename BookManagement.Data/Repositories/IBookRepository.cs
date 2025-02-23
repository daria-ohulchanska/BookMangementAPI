using BookManagement.Data.Entities;
using BookManagement.Shared.Models;

namespace BookManagement.Data.Repositories;

public interface IBookRepository
{
    Task<BookEntity?> GetAsync(int id);
    Task<List<string>> GetTitlePageAsync(PaginationParams paginationParams);
    Task AddAsync(BookEntity book);
    Task AddRangeAsync(List<BookEntity> books);
    Task IncrementViewsAsync(BookEntity book);
    Task<bool> ExistsAsync(string title);
    Task UpdateAsync(BookEntity book);
    Task SoftDeleteAsync(int id);
    Task SoftDeleteAsync(List<int> ids);
}
