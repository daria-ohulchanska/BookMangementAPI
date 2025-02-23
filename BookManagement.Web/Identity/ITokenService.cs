using BookManagementAPI.Models;

namespace BookManagementAPI.Identity;

public interface ITokenService
{
    public AuthenticationTokens GenerateTokens(string userId, string email, IList<string> roles);
}