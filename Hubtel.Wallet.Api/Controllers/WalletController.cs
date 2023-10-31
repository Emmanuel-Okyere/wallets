using Hubtel.Wallet.Api.Dto;
using Hubtel.Wallet.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Wallet.Api.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddWallet([FromBody] WalletRequestDto walletRequestDto, [FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        return Created("", await _walletService.AddWallet(walletRequestDto, authorizationHeader));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWalletById([FromRoute] int id, [FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        return Ok(await _walletService.GetWalletById(id,authorizationHeader));
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllWallets([FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        return Ok(await _walletService.GetAllWallets(authorizationHeader));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWalletById([FromRoute] int id,
        [FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        return Ok(await _walletService.DeleteWalletById(id, authorizationHeader));
    }
}