using Hubtel.Wallet.Api.Dto;
using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Services.Interfaces;

public interface IAuthenticationService
{
    Task<UserResponseDto> Register(RegisterRequestDto registerRequestDto);
    Task<UserResponseDto> Login(LoginDto loginDto);
    Task<User> GetUserFromHeader(string authToken);
}