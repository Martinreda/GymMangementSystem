using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.ToTable("HealthRecords") 
            .HasKey(x => x.Id);

        builder.HasOne<Member>()
            .WithOne(x => x.HealthRecord)
            .HasForeignKey<HealthRecord>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade); 

        
        builder.Property(x => x.Height)
            .HasPrecision(5, 2);

        builder.Property(x => x.Weight)
            .HasPrecision(5, 2);

        builder.Property(x => x.BloodType)
            .HasColumnType("varchar")
            .HasMaxLength(5);

        builder.Property(x => x.Note)
            .HasColumnType("varchar")
            .HasMaxLength(500);

        
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnUpdate();
    }
}