using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Dto;

public class UserResponseDto:MessageResponseDto
{
    public string? Token { get; set; }
    public User? Data { get; set; }
}