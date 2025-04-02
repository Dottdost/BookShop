using AutoMapper;
using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.ModelsAuth;
using BookShop.Auth.ServicesAuth.Interfaces;
using BookShop.Data;
using BookShop.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Auth.ServicesAuth.Classes;


public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly LibraryContext _context;
    private readonly ITokenService _tokenService;

    public AccountService(IMapper mapper, LibraryContext context, ITokenService tokenService)
    {
        _mapper = mapper;
        _context = context;
        _tokenService = tokenService;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var user = _mapper.Map<User>(request);
        user.PasswordHash = HashPassword(user.PasswordHash);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        return passwordHasher.HashPassword(null, password);
    }

    public async Task ConfirmEmailAsync(ConfirmRequest request, HttpContext context)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
        
        // Send email confirmation logic
    }

    public async Task VerifyEmailAsync(string token)
    {
        var username = await _tokenService.GetNameFromToken(token);
        // Verify email with token
    }
}