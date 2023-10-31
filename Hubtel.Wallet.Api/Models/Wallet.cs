using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hubtel.Wallet.Api.Enums;
using Newtonsoft.Json;

namespace Hubtel.Wallet.Api.Models;
[Table("wallets")]
public class Wallet
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required] 
    public string Name { get; set; }
    [Required] 
    public string AccountNumber { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    [Required]
    public WalletType WalletType { get; set; }
    [Required]
    public WalletScheme WalletScheme { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}