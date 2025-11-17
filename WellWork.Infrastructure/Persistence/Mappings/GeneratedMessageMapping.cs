using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WellWork.Domain;

namespace WellWork.Infrastructure.Persistence.Mappings;

public class GeneratedMessageMapping : IEntityTypeConfiguration<GeneratedMessage>
{
    public void Configure(EntityTypeBuilder<GeneratedMessage> builder)
    {
        builder.ToTable("T_WELLWORK_GENERATED_MESSAGES");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("ID")
            .HasColumnType("RAW(16)")
            .HasDefaultValueSql("SYS_GUID()")
            .IsRequired();

        builder.Property(m => m.CheckInId)
            .HasColumnName("CHECKIN_ID")
            .HasColumnType("RAW(16)")
            .IsRequired();

        builder.Property(m => m.Message)
            .HasColumnName("MESSAGE")
            .HasColumnType("CLOB") // ideal para mensagens longas
            .IsRequired();

        builder.Property(m => m.Confidence)
            .HasColumnName("CONFIDENCE")
            .HasColumnType("NUMBER(5,4)") // ex: 0.9876
            .IsRequired();

        builder.Property(m => m.GeneratedAt)
            .HasColumnName("GENERATED_AT")
            .HasColumnType("TIMESTAMP WITH TIME ZONE")
            .IsRequired();

        // Relacionamento 1:1 com CheckIn
        builder.HasOne(m => m.CheckIn)
            .WithOne(c => c.GeneratedMessage)
            .HasForeignKey<GeneratedMessage>(m => m.CheckInId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}