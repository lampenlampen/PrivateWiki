﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StorageProvider;

namespace StorageProvider.Migrations
{
    [DbContext(typeof(PageContext))]
    [Migration("20181020135141_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("StorageProvider.ContentPage", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("ChangeTime");

                    b.Property<string>("Content");

                    b.Property<DateTimeOffset>("CreationTime");

                    b.Property<bool>("IsFavorite");

                    b.Property<bool>("IsLocked");

                    b.Property<DateTimeOffset>("ValidFrom");

                    b.Property<DateTimeOffset>("ValidTo");

                    b.HasKey("Id");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("StorageProvider.Tag", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentPageId");

                    b.Property<string>("TagName");

                    b.HasKey("Name");

                    b.HasIndex("ContentPageId");

                    b.HasIndex("TagName");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("StorageProvider.Tag", b =>
                {
                    b.HasOne("StorageProvider.ContentPage")
                        .WithMany("Tags")
                        .HasForeignKey("ContentPageId");

                    b.HasOne("StorageProvider.Tag")
                        .WithMany("ChildTags")
                        .HasForeignKey("TagName");
                });
#pragma warning restore 612, 618
        }
    }
}
