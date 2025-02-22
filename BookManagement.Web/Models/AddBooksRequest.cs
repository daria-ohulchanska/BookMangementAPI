namespace BookManagementAPI.Models;

public class AddBooksRequest
{
    public List<AddBookRequest> Books { get; set; }
}