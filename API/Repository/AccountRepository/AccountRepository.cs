using API.Data;
using API.Dtos;
using API.Models;
using API.Services;
using API.Utils;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Repository.AccountRepository;

public interface IAccountRepository
{
    Task<ResponseManager> Register(RegisterDto registerDto);
    Task<ResponseManager> Login(LoginDto loginDto);

}

public class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountRepository(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<ResponseManager> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto))
        {
            return new ResponseManager
            {
                Message = $"A user with {registerDto.UserName} found - Go to " +
                          $"the login page!",
                IsSuccess = false
            };
        }

        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return new ResponseManager
            {
                Message = "Passwords do not match",
                IsSuccess = false
            };
        }

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        var result = await _context.SaveChangesAsync() > 0;
        if (!result)
        {
            return new ResponseManager
            {
                Message = "Problem creating a user",
                IsSuccess = false
            };
        }

        return new ResponseManager
        {
            UserId = user.UserId,
            Message = "Registration succeeded",
            IsSuccess = true
        };
    }


    public async Task<ResponseManager> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
        if (user is null)
        {
            return new ResponseManager
            {
                Message = "Invalid username",
                IsSuccess = false
            };
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
                return new ResponseManager
                {
                    Message = "Either username or Password is invalid",
                    IsSuccess = false
                };
        }

        return new ResponseManager
        {
            UserName = user.UserName,
            UserId = user.UserId,
            Token = _tokenService.CreateToken(user),
            Message = "Login succeeded",
            IsSuccess = true

        };

    }

    private async Task<bool> UserExists(RegisterDto registerDto)
    {
        return await _context.Users.AnyAsync(u =>
               u.UserName == registerDto.UserName.ToLower());

    }


}
