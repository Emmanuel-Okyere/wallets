using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Hubtel.Wallet.Api.Models;
[Table("users")]
public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    [JsonIgnore]
    public byte[] PasswordHash { get; set; }
    [Required]
    [JsonIgnore]
    public byte[] PasswordSalt { get; set; }
    public List<Wallet> Wallets { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}