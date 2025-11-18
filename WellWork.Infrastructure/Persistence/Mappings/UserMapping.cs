using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellWork.Domain;

namespace WellWork.Infrastructure.Persistence.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("USERS");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("ID")
            .HasColumnType("RAW(16)")
            .IsRequired()
            .HasDefaultValueSql("SYS_GUID()");

        builder.Property(u => u.Username)
            .HasColumnName("USERNAME")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("PASSWORD")
            .HasMaxLength(500)
            .IsRequired();
        

        // User 1 -> N CheckIns
        builder.HasMany(u => u.CheckIns)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}