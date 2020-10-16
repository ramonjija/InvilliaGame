using Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(c => c.UserId);
            builder.Property(c => c.UserName);
            builder.Property(c => c.PasswordHash);
            builder.HasOne(c => c.UserType);
        }
    }
}
