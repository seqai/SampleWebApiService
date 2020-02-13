﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SampleWebApiService.DataAccess;

namespace SampleWebApiService.Migrations
{
    [DbContext(typeof(ServiceSqlLiteDbContext))]
    [Migration("20200213092935_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("SampleWebApiService.DataAccess.Entities.CalendarEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EventOrganizer")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Time")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("CalendarEvents");
                });

            modelBuilder.Entity("SampleWebApiService.DataAccess.Entities.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("SampleWebApiService.DataAccess.Entities.Relations.CalendarEventMember", b =>
                {
                    b.Property<int>("CalendarEventId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MemberId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CalendarEventId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("CalendarEventMember");
                });

            modelBuilder.Entity("SampleWebApiService.DataAccess.Entities.Relations.CalendarEventMember", b =>
                {
                    b.HasOne("SampleWebApiService.DataAccess.Entities.CalendarEvent", "CalendarEvent")
                        .WithMany("CalendarEventMembers")
                        .HasForeignKey("CalendarEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SampleWebApiService.DataAccess.Entities.Member", "Member")
                        .WithMany("CalendarEventMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}