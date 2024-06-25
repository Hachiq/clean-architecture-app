using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username).HasMaxLength(50).UseCollation("SQL_Latin1_General_CP1_CS_AS").IsRequired();
            builder.Property(u => u.Email).HasMaxLength(254).UseCollation("SQL_Latin1_General_CP1_CS_AS").IsRequired();
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.PasswordSalt).IsRequired();

            builder.HasOne(u => u.RefreshToken).WithOne();
        }
    }
}
