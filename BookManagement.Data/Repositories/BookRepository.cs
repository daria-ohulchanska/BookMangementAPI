using BookManagement.Data.Contexts;
using BookManagement.Data.Entities;
using BookManagement.Data.Extensions;
using BookManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Data.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BookEntity?> GetAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }
    
    public async Task<List<string>> GetTitlePageAsync(PaginationParams paginationParams)
    {
        var query = _context.Books.Select(b => new
            {
                b.Title,
                PopularityScore = b.CalculatePopularity()
            })
            .OrderByDescending(b => b.PopularityScore)
            .Select(b => b.Title);

        query = query.Paginate(paginationParams.PageNumber, paginationParams.PageSize);

        var books = await query.ToListAsync();
        
        return books;
    }

    public async Task AddAsync(BookEntity book)
    {
        book.CreatedAt = DateTime.UtcNow;
        book.UpdatedAt = DateTime.UtcNow;
        
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddRangeAsync(List<BookEntity> books)
    {
        books.ForEach(x => x.UpdatedAt = DateTime.UtcNow);
        _context.Books.AddRange(books);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsAsync(string title)
    {
        return await _context.Books.AnyAsync(b => b.Title == title);
    }
    
    public async Task UpdateAsync(BookEntity book)
    {
        book.UpdatedAt = DateTime.Now;
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }
    
    public async Task SoftDeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null) return;

        book.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
    
    public async Task SoftDeleteAsync(List<int> ids)
    {
        var books = await _context.Books
            .Where(b => ids.Contains(b.Id))
            .ToListAsync();
        
        if (books.Count == 0) 
            return;

        books.ForEach(b => b.IsDeleted = true);
        await _context.SaveChangesAsync();
    }
    
    public async Task IncrementViewsAsync(BookEntity book)
    {
        book.ViewsCount++;
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }
}
