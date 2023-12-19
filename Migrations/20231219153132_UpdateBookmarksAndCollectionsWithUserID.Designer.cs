﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Website_Bookmark_Desktop_App_API.Data;

#nullable disable

namespace Website_Bookmark_Desktop_App_API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231219153132_UpdateBookmarksAndCollectionsWithUserID")]
    partial class UpdateBookmarksAndCollectionsWithUserID
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Website_Bookmark_Desktop_App_API.Models.BookmarkCollection", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CollectionTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OwningCollectionID")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Website_Bookmark_Desktop_App_API.Models.DesktopBookmark", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BookmarkURL")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CollectionID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("URLTitle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Bookmarks");
                });
#pragma warning restore 612, 618
        }
    }
}