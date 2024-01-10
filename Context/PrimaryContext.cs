using EfCoreBugs.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBugs.Context;

public class PrimaryContext(DbContextOptions<PrimaryContext> options) : DbContext(options)
{
    public virtual DbSet<TestItem> TestItems { get; set; } = null!;
}