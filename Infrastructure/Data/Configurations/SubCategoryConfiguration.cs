using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.Property(sc => sc.Title).HasMaxLength(50).IsRequired();

            builder.HasOne(sc => sc.Category).WithMany(c => c.SubCategories).HasForeignKey(sc => sc.CategoryId);
        }
    }
}
