﻿// <auto-generated />
using System;
using InstagramClone.Infrastructure.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InstagramClone.Infrastructure.Migrations
{
    [DbContext(typeof(InstagramCloneDbContext))]
    partial class InstagramCloneDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.Post.PostComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CommentCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CommentEditedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.Post.PostLike", b =>
                {
                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("UserProfileId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.Post.UserPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedUserProfileId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("PostCreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PostEditedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserProfileId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.User.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.Post.PostComment", b =>
                {
                    b.HasOne("InstagramClone.Domain.DAL.Models.Post.UserPost", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Domain.DAL.Models.User.UserProfile", "UserProfile")
                        .WithMany("Comments")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.Post.PostLike", b =>
                {
                    b.HasOne("InstagramClone.Domain.DAL.Models.Post.UserPost", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstagramClone.Domain.DAL.Models.User.UserProfile", "UserProfile")
                        .WithMany("Likes")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.Post.UserPost", b =>
                {
                    b.HasOne("InstagramClone.Domain.DAL.Models.User.UserProfile", "CreatedUserProfile")
                        .WithMany("Posts")
                        .HasForeignKey("CreatedUserProfileId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("CreatedUserProfile");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.Post.UserPost", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("InstagramClone.Domain.DAL.Models.User.UserProfile", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
