using Hubtel.Wallet.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallet.Api.Data;

public class DataContext:DbContext
{
    
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Models.Wallet> Wallets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}