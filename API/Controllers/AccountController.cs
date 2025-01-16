using API.Dtos;
using API.Repository.AccountRepository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController
              (IAccountRepository accountRepo,
               ILogger<AccountController> log) : ControllerBase
{
    private readonly IAccountRepository _accountRepo = accountRepo;
    private readonly ILogger<AccountController> _log = log;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _accountRepo.Register(registerDto);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        _log.LogInformation($"Registration attempt by {registerDto.UserName} failed");
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _accountRepo.Login(loginDto);
        if (result.IsSuccess) return Ok(result);

        _log.LogInformation($"Login attempt by {loginDto.UserName} failed");
        return Unauthorized(result);
    }
}
