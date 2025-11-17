using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellWork.Domain;

namespace WellWork.Infrastructure.Persistence.Mappings;

public class CheckInMapping : IEntityTypeConfiguration<CheckIn>
{
    public void Configure(EntityTypeBuilder<CheckIn> builder)
    {
        builder.ToTable("T_WELLWORK_CHECKINS");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("ID")
            .HasColumnType("RAW(16)")
            .IsRequired()
            .HasDefaultValueSql("SYS_GUID()");

        builder.Property(c => c.UserId)
            .HasColumnName("USER_ID")
            .HasColumnType("RAW(16)")
            .IsRequired();

        // 🔥 ENUM: Mood -> VARCHAR2
        builder.Property(c => c.Mood)
            .HasColumnName("MOOD")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // 🔥 ENUM: Energy -> VARCHAR2
        builder.Property(c => c.Energy)
            .HasColumnName("ENERGY")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Notes)
            .HasColumnName("NOTES")
            .HasMaxLength(1000);

        builder.Property(c => c.CreatedAt)
            .HasColumnName("CREATED_AT")
            .HasColumnType("TIMESTAMP WITH TIME ZONE")
            .IsRequired();

        // Relacionamento: User 1 -> N CheckIns
        builder.HasOne(c => c.User)
            .WithMany(u => u.CheckIns)
            .HasForeignKey(c => c.UserId);

        // Relacionamento: CheckIn 1 -> 1 GeneratedMessage
        builder.HasOne(c => c.GeneratedMessage)
            .WithOne(m => m.CheckIn)
            .HasForeignKey<GeneratedMessage>(m => m.CheckInId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}