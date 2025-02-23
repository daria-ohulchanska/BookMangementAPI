using BookManagement.Core.Models;
using BookManagement.Data.Entities;
using BookManagement.Data.Extensions;
using BookManagement.Data.Repositories;
using BookManagement.Shared.Models;

namespace BookManagement.Core.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BookModel> AddAsync(BookModel model)
    {
        if (await _bookRepository.ExistsAsync(model.Title))
            throw new Exception($"Title already exists: {model.Title}");

        var entity = new BookEntity
        {
            Title = model.Title,
            Author = model.Author,
            PublicationYear = model.PublicationYear
        };
        
        await _bookRepository.AddAsync(entity);
        
        return model;
    }
    
    public async Task<List<BookModel>> AddRangeAsync(List<BookModel> models)
    {
        foreach (var model in models)
        {
            if (await _bookRepository.ExistsAsync(model.Title))
                throw new Exception($"A book with the title '{model.Title}' already exists.");
        }

        var entities = models.Select(x => new BookEntity()
        {
            Title = x.Title,
            Author = x.Author,
            PublicationYear = x.PublicationYear
        });

        await _bookRepository.AddRangeAsync(entities.ToList());

        return models;
    }

    public async Task<DetailBookModel?> GetAsync(int id)
    {
        var entity = await _bookRepository.GetAsync(id);
        if (entity == null) 
            return null;
        
        await _bookRepository.IncrementViewsAsync(entity);
        
        var model = new DetailBookModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Author = entity.Author,
            PublicationYear = entity.PublicationYear,
        };

        model.PopularityScore = entity.CalculatePopularity();
        
        return model;
    }
    
    public async Task<List<string>> GetTitlePageAsync(PaginationParams paginationParams)
    {
        var titles = await _bookRepository.GetTitlePageAsync(paginationParams);
        return titles;
    }
    
    public async Task UpdateAsync(BookModel model)
    {
        var entity = new BookEntity()
        {
            Id = model.Id,
            Title = model.Title,
            Author = model.Author,
            PublicationYear = model.PublicationYear
        };

        await _bookRepository.UpdateAsync(entity);
    }

    public async Task SoftDeleteAsync(int id)
    {
        await _bookRepository.SoftDeleteAsync(id);
    }

    public async Task SoftDeleteAsync(List<int> ids)
    {
        await _bookRepository.SoftDeleteAsync(ids);
    }
}