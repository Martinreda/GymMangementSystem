using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(X => X.Email)
              .HasColumnType("varchar")
              .HasMaxLength(100);
            builder.Property(X => X.Phone)
             .HasColumnType("varchar")
             .HasMaxLength(11);


            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymValidEmailCheck", "Email Like '_%@_%._%'");
                Tb.HasCheckConstraint("GymValidPhoneCheck", "Phone Like '01%' and Phone not Like '%[^0-9]%'");
            });
            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();

            builder.OwnsOne(X => X.Address, AddressBuilder =>
            {
                AddressBuilder.Property(X => X.Street)
                .HasColumnType("varchar")
                .HasColumnName("Street")
                .HasMaxLength(30);

                AddressBuilder.Property(X => X.City)
             .HasColumnType("varchar")
             .HasColumnName("City")
             .HasMaxLength(30);

                AddressBuilder.Property(X => X.BulidingNumber)
             .HasColumnName("BulidingNumber");

            });
        }
    }
}
