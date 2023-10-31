using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallet.Api.Data;

public class DataContext:DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}