﻿// <auto-generated />
using System;
using DropWord.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DropWord.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DropWord.Domain.Entities.SentenceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sentence")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Sentence", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.SentencesPairEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FirstLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FirstSentenceId")
                        .HasColumnType("int");

                    b.Property<string>("SecondLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SecondSentenceId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("FirstSentenceId");

                    b.HasIndex("SecondSentenceId");

                    b.HasIndex("UserId");

                    b.ToTable("SentencesPair", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.StateTreeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("JsonData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JsonTempData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("StateTree", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserLearningInfoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CountUseForDaySentences")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("LastUseForDaySentencesId")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserLearningInfo", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserSentencesCollectionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSentencesCollection", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserSettingsEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("InterfaceLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LearnLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LearnSentencesModeEnum")
                        .HasColumnType("int");

                    b.Property<string>("MainLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SentencesRepeatForDayModeEnum")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UsingSentencesPairEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountUse")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsLearning")
                        .HasColumnType("bit");

                    b.Property<int>("SentencesPairId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("WhenDeleted")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("SentencesPairId");

                    b.HasIndex("UserId");

                    b.ToTable("UsingSentencesPair", (string)null);
                });

            modelBuilder.Entity("SentencesPairEntityUserSentencesCollectionEntity", b =>
                {
                    b.Property<int>("SentencesPairsId")
                        .HasColumnType("int");

                    b.Property<int>("UserSentencesCollectionsId")
                        .HasColumnType("int");

                    b.HasKey("SentencesPairsId", "UserSentencesCollectionsId");

                    b.HasIndex("UserSentencesCollectionsId");

                    b.ToTable("SentencesPairEntityUserSentencesCollectionEntity", (string)null);
                });

            modelBuilder.Entity("DropWord.Domain.Entities.SentencesPairEntity", b =>
                {
                    b.HasOne("DropWord.Domain.Entities.SentenceEntity", "FirstSentence")
                        .WithMany("FirstSentencesPairs")
                        .HasForeignKey("FirstSentenceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DropWord.Domain.Entities.SentenceEntity", "SecondSentence")
                        .WithMany("SecondSentencesPairs")
                        .HasForeignKey("SecondSentenceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DropWord.Domain.Entities.UserEntity", "User")
                        .WithMany("SentencesPairs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FirstSentence");

                    b.Navigation("SecondSentence");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DropWord.Domain.Entities.StateTreeEntity", b =>
                {
                    b.HasOne("DropWord.Domain.Entities.UserEntity", "User")
                        .WithOne("StateTree")
                        .HasForeignKey("DropWord.Domain.Entities.StateTreeEntity", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserLearningInfoEntity", b =>
                {
                    b.HasOne("DropWord.Domain.Entities.UserEntity", "User")
                        .WithOne("UserLearningInfo")
                        .HasForeignKey("DropWord.Domain.Entities.UserLearningInfoEntity", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserSentencesCollectionEntity", b =>
                {
                    b.HasOne("DropWord.Domain.Entities.UserEntity", "User")
                        .WithMany("UserSentencesCollections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserSettingsEntity", b =>
                {
                    b.HasOne("DropWord.Domain.Entities.UserEntity", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("DropWord.Domain.Entities.UserSettingsEntity", "UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UsingSentencesPairEntity", b =>
                {
                    b.HasOne("DropWord.Domain.Entities.SentencesPairEntity", "SentencesPair")
                        .WithMany("UsingSentencesPairs")
                        .HasForeignKey("SentencesPairId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DropWord.Domain.Entities.UserEntity", "User")
                        .WithMany("UsingSentencesPairs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SentencesPair");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SentencesPairEntityUserSentencesCollectionEntity", b =>
                {
                    b.HasOne("DropWord.Domain.Entities.SentencesPairEntity", null)
                        .WithMany()
                        .HasForeignKey("SentencesPairsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DropWord.Domain.Entities.UserSentencesCollectionEntity", null)
                        .WithMany()
                        .HasForeignKey("UserSentencesCollectionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DropWord.Domain.Entities.SentenceEntity", b =>
                {
                    b.Navigation("FirstSentencesPairs");

                    b.Navigation("SecondSentencesPairs");
                });

            modelBuilder.Entity("DropWord.Domain.Entities.SentencesPairEntity", b =>
                {
                    b.Navigation("UsingSentencesPairs");
                });

            modelBuilder.Entity("DropWord.Domain.Entities.UserEntity", b =>
                {
                    b.Navigation("SentencesPairs");

                    b.Navigation("StateTree")
                        .IsRequired();

                    b.Navigation("UserLearningInfo")
                        .IsRequired();

                    b.Navigation("UserSentencesCollections");

                    b.Navigation("UserSettings")
                        .IsRequired();

                    b.Navigation("UsingSentencesPairs");
                });
#pragma warning restore 612, 618
        }
    }
}
