using Hubtel.Wallet.Api.Dto;
using Hubtel.Wallet.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Wallet.Api.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class AuthenticationController: ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        return Created("", await _authenticationService.Register(registerRequestDto));
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        return Ok(await _authenticationService.Login(loginDto));
    }
}