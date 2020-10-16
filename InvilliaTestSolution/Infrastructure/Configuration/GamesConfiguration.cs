using Domain.Model.Aggregate;
using Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Configuration
{
    public class GamesConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(c => c.GameId);
            builder.HasIndex(c => c.GameName).IsUnique();
            builder.Property(c => c.Available);
            builder
                .HasOne(d => d.BorrowedGame)
                .WithOne(c => c.Game)
                .HasForeignKey<BorrowedGame>(d => d.BorrowedGameId);

        }
    }
}
