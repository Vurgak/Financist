﻿// <auto-generated />
using System;
using Financist.WebApi.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Financist.WebApi.Users.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    [Migration("20240811130941_AddRefreshTokensTable")]
    partial class AddRefreshTokensTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("users")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Financist.WebApi.Users.Domain.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("GeneratedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("ValidUntil")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("RefreshTokens", "users");
                });

            modelBuilder.Entity("Financist.WebApi.Users.Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users", "users");
                });
#pragma warning restore 612, 618
        }
    }
}
