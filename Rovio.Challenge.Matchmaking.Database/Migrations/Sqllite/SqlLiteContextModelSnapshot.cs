﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rovio.Challenge.Matchmaking.Database.DbContexts;

#nullable disable

namespace Rovio.Challenge.Matchmaking.Database.Migrations.Sqllite
{
    [DbContext(typeof(SqlLiteContext))]
    partial class SqlLiteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Game", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Player", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<int>("Latency")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Region")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("SessionId")
                        .HasColumnType("TEXT");

                    b.HasKey("Username");

                    b.HasIndex("SessionId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Server", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Region")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameName")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameName");

                    b.HasIndex("ServerId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Player", b =>
                {
                    b.HasOne("Rovio.Challenge.Matchmaking.Domain.Models.Session", null)
                        .WithMany("Players")
                        .HasForeignKey("SessionId");
                });

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Session", b =>
                {
                    b.HasOne("Rovio.Challenge.Matchmaking.Domain.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameName");

                    b.HasOne("Rovio.Challenge.Matchmaking.Domain.Models.Server", "Server")
                        .WithMany("Sessions")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Server", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Rovio.Challenge.Matchmaking.Domain.Models.Session", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
