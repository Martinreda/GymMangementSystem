using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.CategoryName)
            .HasColumnType("varchar")
            .HasMaxLength(20)
            .IsRequired(); 

        builder.HasIndex(x => x.CategoryName)
            .IsUnique(); // منع تكرار الأسماء
    }
}