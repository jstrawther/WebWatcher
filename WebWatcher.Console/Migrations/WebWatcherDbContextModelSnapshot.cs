﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebWatcher.EF;

namespace WebWatcher.Console.Migrations
{
    [DbContext(typeof(WebWatcherDbContext))]
    partial class WebWatcherDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("WebWatcher.Core.Models.Website", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ElementSelector")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Websites");
                });

            modelBuilder.Entity("WebWatcher.Core.Models.WebsiteSnapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<int>("WebsiteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("WebsiteSnapshots");
                });

            modelBuilder.Entity("WebWatcher.Core.Models.WebsiteSubscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("TEXT");

                    b.Property<int>("WebsiteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("WebsiteSubscribers");
                });

            modelBuilder.Entity("WebWatcher.Core.Models.WebsiteSnapshot", b =>
                {
                    b.HasOne("WebWatcher.Core.Models.Website", "Website")
                        .WithMany("Snapshots")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Website");
                });

            modelBuilder.Entity("WebWatcher.Core.Models.WebsiteSubscriber", b =>
                {
                    b.HasOne("WebWatcher.Core.Models.Website", "Website")
                        .WithMany("Subscribers")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Website");
                });

            modelBuilder.Entity("WebWatcher.Core.Models.Website", b =>
                {
                    b.Navigation("Snapshots");

                    b.Navigation("Subscribers");
                });
#pragma warning restore 612, 618
        }
    }
}
