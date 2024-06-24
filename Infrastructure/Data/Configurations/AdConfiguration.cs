using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AdConfiguration : IEntityTypeConfiguration<Ad>
    {
        public void Configure(EntityTypeBuilder<Ad> builder)
        {
            builder.Property(a => a.Title).HasMaxLength(50).IsRequired();
            builder.Property(a => a.Price).HasMaxLength(50).IsRequired();

            builder.HasOne(a => a.User).WithMany(u => u.Ads).HasForeignKey(a => a.UserId);
            builder.HasOne(a => a.SubCategory).WithMany(sc => sc.Ads).HasForeignKey(a => a.SubCategoryId);
        }
    }
}
