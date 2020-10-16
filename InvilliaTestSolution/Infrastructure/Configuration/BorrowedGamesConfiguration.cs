using Domain.Model.Aggregate;
using Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Configuration
{
    public class BorrowedGamesConfiguration : IEntityTypeConfiguration<BorrowedGame>
    {
        public void Configure(EntityTypeBuilder<BorrowedGame> builder)
        {
            builder
                .HasKey(c => c.BorrowedGameId);

            builder
                .Property(c => c.BorrowDate);

            builder
                .HasOne(c => c.Friend)
                .WithMany(c => c.BorrowedGames);

            builder
                .HasOne(c => c.Game)
                .WithOne(c => c.BorrowedGame);
        }
    }
}
