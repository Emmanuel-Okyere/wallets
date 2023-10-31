using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallet.Api.Dto;

public class RegisterRequestDto:LoginDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}