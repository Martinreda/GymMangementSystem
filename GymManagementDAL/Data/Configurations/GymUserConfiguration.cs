using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnType("varchar")
            .HasMaxLength(11)
            .IsRequired();

        // تحسين الـ Check Constraints
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint("GymValidEmailCheck", "Email LIKE '_%@_%._%' AND Email NOT LIKE '% %'");
            tb.HasCheckConstraint("GymValidPhoneCheck", "Phone LIKE '01[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
        });

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Phone).IsUnique();

        // إضافة CreatedAt و UpdatedAt لو مش موجودين في الأساسي
        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("GETDATE()")
            .ValueGeneratedOnUpdate();

        builder.OwnsOne(x => x.Address, addressBuilder =>
        {
            addressBuilder.Property(x => x.Street)
                .HasColumnType("varchar")
                .HasColumnName("Street")
                .HasMaxLength(30);

            addressBuilder.Property(x => x.City)
                .HasColumnType("varchar")
                .HasColumnName("City")
                .HasMaxLength(30)
                .IsRequired();

            addressBuilder.Property(x => x.BulidingNumber)
                .HasColumnName("BuildingNumber")
                .IsRequired();
        });
    }
}