using Microsoft.EntityFrameworkCore;

namespace Backend;

public sealed class ItemsContext : DbContext
{
    public ItemsContext(DbContextOptions<ItemsContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Item> Items { get; init; }
}