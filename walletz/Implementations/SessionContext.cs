using Microsoft.EntityFrameworkCore;
using walletz.Models;

namespace walletz.Implementations;


public class SessionContext : DbContext
{

    public DbSet<User> users { get; set; }
    public DbSet<Wallet> wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite(@"Data Source=Wallet.db;");

    }

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        modelbuilder.Entity<User>(u =>
        {
            u.HasKey(u => u.Id);
            u.Property(u => u.CreatedAt).HasDefaultValueSql("DATETIME('now')");
        });

        modelbuilder.Entity<Wallet>(w =>
        {
            w.HasKey(w => w.Id);
            w.Property(w => w.CreatedAt).HasDefaultValueSql("DATETIME('now')");

        });
    }


}
