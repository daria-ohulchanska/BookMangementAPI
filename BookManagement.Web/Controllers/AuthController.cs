using BookManagementAPI.Identity;
using BookManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync(SignUpRequest request)
    {
        await _identityService.SignUpAsync(request.UserName, request.Email, request.Password);
        return Ok();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync(SignInRequest request)
    {
        var response = await _identityService.SignInAsync(request.Email, request.Password);
        return Ok(response);
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOutAsync(string email)
    {
        await _identityService.SignOutAsync(email);
        return RedirectToAction("Index", "Home");
    }
}