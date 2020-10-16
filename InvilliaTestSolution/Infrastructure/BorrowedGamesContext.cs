using DataAccess.Configuration;
using Domain.Model.Aggregate;
using Domain.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
    public class BorrowedGamesContext : DbContext
    {
        public BorrowedGamesContext()
        {
        }

        public BorrowedGamesContext(DbContextOptions<BorrowedGamesContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<BorrowedGame> BorrowedGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FriendsConfiguration());
            modelBuilder.ApplyConfiguration(new GamesConfiguration());
            modelBuilder.ApplyConfiguration(new BorrowedGamesConfiguration());
        }
    }
}
