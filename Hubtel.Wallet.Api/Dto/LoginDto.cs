using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallet.Api.Dto;

public class LoginDto
{
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    [Required]
    public string Password { get; set; }
}