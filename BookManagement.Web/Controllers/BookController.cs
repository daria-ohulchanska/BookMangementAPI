using BookManagement.Core.Models;
using BookManagement.Core.Services;
using BookManagement.Shared.Models;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookModel>> Get(int id)
    {
        var book = await _bookService.GetAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }
    
    [HttpGet("titles")]
    public async Task<ActionResult<PopularTitlesResponse>> GetTitles(int page = 1, int pageSize = 10)
    {
        var paginationParams = new PaginationParams
        {
            PageNumber = page,
            PageSize = pageSize,
        };
        
        var paginatedTitles = await _bookService.GetTitlePageAsync(paginationParams);
        var popularTitlesResponse = new PopularTitlesResponse { Titles = paginatedTitles };
        
        return Ok(popularTitlesResponse);
    }
    
    [HttpPost]
    public async Task<ActionResult<BookModel>> Add(AddBookRequest request)
    {
        if (request.Title.IsNullOrEmpty())
            return BadRequest("Invalid title");
        if (request.Author.IsNullOrEmpty())
            return BadRequest("Invalid author");
        if (request.PublicationYear > DateTime.Now.Year || request.PublicationYear < 0)
            return BadRequest("Invalid year");

        var model = new BookModel
        {
            Title = request.Title,
            Author = request.Author,
            PublicationYear = request.PublicationYear,
        };
        
        var added = await _bookService.AddAsync(model);

        return Created(nameof(Get), added);
    }

    [HttpPost("add-range")]
    public async Task<ActionResult<List<BookModel>>> AddRange([FromQuery]AddBooksRequest request)
    {
        if (request.Books.Any(book => book.Title.IsNullOrEmpty()
                                      || book.Author.IsNullOrEmpty()
                                      || book.PublicationYear > DateTime.Now.Year
                                      || book.PublicationYear < 0))
        {
            return BadRequest("Request contains invalid input data");
        }
        
        var bookModels = request.Books.Select(x => new BookModel
        {
            Title = x.Title,
            Author = x.Author,
            PublicationYear = x.PublicationYear,
        });
        
        var added = await _bookService.AddRangeAsync(bookModels.ToList());
        
        return Created(nameof(Get), added);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateBookRequest book)
    {
        var bookModel = new BookModel
        {
            Title = book.Title,
            Author = book.Author,
            PublicationYear = book.PublicationYear,
        };
        
        await _bookService.UpdateAsync(bookModel);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bookService.SoftDeleteAsync(id);
        return NoContent();
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] List<int> ids)
    {
        await _bookService.SoftDeleteAsync(ids);
        return NoContent();
    }
}