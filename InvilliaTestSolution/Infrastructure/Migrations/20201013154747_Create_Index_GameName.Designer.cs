﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(BorrowedGamesContext))]
    [Migration("20201013154747_Create_Index_GameName")]
    partial class Create_Index_GameName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Model.Aggregate.BorrowedGame", b =>
                {
                    b.Property<int>("BorrowedGameId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FriendUserId")
                        .HasColumnType("int");

                    b.HasKey("BorrowedGameId");

                    b.HasIndex("FriendUserId");

                    b.ToTable("BorrowedGames");
                });

            modelBuilder.Entity("Domain.Model.Entity.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Available")
                        .HasColumnType("bit");

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("GameId");

                    b.HasIndex("GameName")
                        .IsUnique()
                        .HasFilter("[GameName] IS NOT NULL");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Domain.Model.Entity.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserTypeTypeId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("UserTypeTypeId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("Domain.Model.Entity.UserType", b =>
                {
                    b.Property<int>("TypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TypeId");

                    b.ToTable("UserTypes");

                    b.HasData(
                        new
                        {
                            TypeId = 1,
                            Type = "Administrator"
                        },
                        new
                        {
                            TypeId = 2,
                            Type = "Friend"
                        });
                });

            modelBuilder.Entity("Domain.Model.Entity.Friend", b =>
                {
                    b.HasBaseType("Domain.Model.Entity.User");

                    b.HasDiscriminator().HasValue("Friend");
                });

            modelBuilder.Entity("Domain.Model.Aggregate.BorrowedGame", b =>
                {
                    b.HasOne("Domain.Model.Entity.Game", "Game")
                        .WithOne("BorrowedGame")
                        .HasForeignKey("Domain.Model.Aggregate.BorrowedGame", "BorrowedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Model.Entity.Friend", "Friend")
                        .WithMany("BorrowedGame")
                        .HasForeignKey("FriendUserId");
                });

            modelBuilder.Entity("Domain.Model.Entity.User", b =>
                {
                    b.HasOne("Domain.Model.Entity.UserType", "UserType")
                        .WithMany()
                        .HasForeignKey("UserTypeTypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
