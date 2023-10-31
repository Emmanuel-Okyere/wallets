using System.ComponentModel.DataAnnotations;
using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Dto;

public class WalletRequestDto
{
    [Required] 
    public string name { get; set; }
    [Required] 
    public string AccountNumber { get; set; }
    [Required]
    public WalletType WalletType { get; set; }
    [Required]
    public WalletScheme WalletScheme { get; set; }
}