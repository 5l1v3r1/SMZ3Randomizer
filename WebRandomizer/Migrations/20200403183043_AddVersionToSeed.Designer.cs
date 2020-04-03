﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebRandomizer.Models;

namespace WebRandomizer.Migrations
{
    [DbContext(typeof(RandomizerContext))]
    [Migration("20200403183043_AddVersionToSeed")]
    partial class AddVersionToSeed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WebRandomizer.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.Property<string>("Device")
                        .HasColumnType("text");

                    b.Property<string>("Guid")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("RecievedSeq")
                        .HasColumnType("integer");

                    b.Property<int>("SentSeq")
                        .HasColumnType("integer");

                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id");

                    b.HasIndex("SessionId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("WebRandomizer.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemIndex")
                        .HasColumnType("integer");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int>("SequenceNum")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("WebRandomizer.Models.Seed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("GameId")
                        .HasColumnType("text");

                    b.Property<string>("GameName")
                        .HasColumnType("text");

                    b.Property<string>("GameVersion")
                        .HasColumnType("text");

                    b.Property<string>("Guid")
                        .HasColumnType("text");

                    b.Property<string>("Mode")
                        .HasColumnType("text");

                    b.Property<int>("Players")
                        .HasColumnType("integer");

                    b.Property<string>("SeedNumber")
                        .HasColumnType("text");

                    b.Property<string>("Spoiler")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Seeds");
                });

            modelBuilder.Entity("WebRandomizer.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Guid")
                        .HasColumnType("text");

                    b.Property<int?>("SeedId")
                        .HasColumnType("integer");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SeedId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("WebRandomizer.Models.World", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Guid")
                        .HasColumnType("text");

                    b.Property<byte[]>("Patch")
                        .HasColumnType("bytea");

                    b.Property<string>("Player")
                        .HasColumnType("text");

                    b.Property<int>("SeedId")
                        .HasColumnType("integer");

                    b.Property<string>("Settings")
                        .HasColumnType("text");

                    b.Property<int>("WorldId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SeedId");

                    b.ToTable("Worlds");
                });

            modelBuilder.Entity("WebRandomizer.Models.Client", b =>
                {
                    b.HasOne("WebRandomizer.Models.Session", null)
                        .WithMany("Clients")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebRandomizer.Models.Event", b =>
                {
                    b.HasOne("WebRandomizer.Models.Client", null)
                        .WithMany("Events")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebRandomizer.Models.Session", b =>
                {
                    b.HasOne("WebRandomizer.Models.Seed", "Seed")
                        .WithMany()
                        .HasForeignKey("SeedId");
                });

            modelBuilder.Entity("WebRandomizer.Models.World", b =>
                {
                    b.HasOne("WebRandomizer.Models.Seed", null)
                        .WithMany("Worlds")
                        .HasForeignKey("SeedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
