using Microsoft.AspNetCore.Mvc;
using Organizarty.Application.Dtos.Requests;
using Organizarty.Domain.Entities;
using Organizarty.Domain.UseCases.Users;

namespace Organizarty.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ISignUseCase _sign;

    public AccountController(ILogger<WeatherForecastController> logger, ISignUseCase sign)
    {
        _logger = logger;
        _sign = sign;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDTO userdto)
    {
        var (user, token) = await _sign.Login(userdto.Email, userdto.Password);

        var data = new
        {
            token = token,
            user = new
            {
                id = user.Id,
                user_name = user.UserName,
                email = user.Email
            }
        };

        return Ok(data);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDTO userdto)
    {
        var user = await _sign.Register(userdto.UserName, userdto.Email, userdto.Password);

        var data = new
        {
            user = new
            {
                id = user.Id,
                user_name = user.UserName,
                email = user.Email
            }
        };

        return Ok(data);
    }
}
