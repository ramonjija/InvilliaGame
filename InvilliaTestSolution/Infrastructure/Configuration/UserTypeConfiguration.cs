using Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Configuration
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.HasKey(c => c.TypeId);
            builder.Property(c => c.Type);
            builder.HasData(new UserType[]
            {
                new UserType(){TypeId = 1,Type = "Administrator" },
                new UserType(){TypeId = 2, Type = "Friend"}
            });
        }
    }
}
