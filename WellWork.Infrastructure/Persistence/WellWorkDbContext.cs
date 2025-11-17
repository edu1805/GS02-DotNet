using Microsoft.EntityFrameworkCore;
using WellWork.Domain;
using WellWork.Infrastructure.Persistence.Mappings;

namespace WellWork.Infrastructure.Persistence;

public class WellWorkDbContext(DbContextOptions<WellWorkDbContext> options) : DbContext(options)
{
    public DbSet<CheckIn> CheckIns { get; set; }
    public DbSet<GeneratedMessage> GeneratedMessages { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CheckInMapping());
        modelBuilder.ApplyConfiguration(new GeneratedMessageMapping());
        modelBuilder.ApplyConfiguration(new UserMapping());
    }
}