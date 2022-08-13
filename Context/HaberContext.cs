using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using haber1.Model;

namespace haber1.Context
{
    public partial class HaberContext : DbContext
    {
        public HaberContext()
        {
        }

        public HaberContext(DbContextOptions<HaberContext> options)
            : base(options)
        {
        }

        public virtual DbSet<News> News { get; set; } = null!;
        public virtual DbSet<NewsCategory> NewsCategories { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("UserID=postgres;Password=122333;Server=localHost;port=5432;Database=HaberSitesi;Pooling=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsId).HasColumnName("news_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.DatePosted)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_posted");

                entity.Property(e => e.NewsContent)
                    .HasColumnType("character varying")
                    .HasColumnName("news_content");

                entity.Property(e => e.NewsStatus).HasColumnName("news_status");

                entity.Property(e => e.NewsTitle)
                    .HasColumnType("character varying")
                    .HasColumnName("news_title");

                entity.Property(e => e.Pictures)
                    .HasColumnType("character varying")
                    .HasColumnName("pictures");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<NewsCategory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NewsCategory");

                entity.Property(e => e.CategoryDescription)
                    .HasColumnType("character varying")
                    .HasColumnName("category_description");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .HasColumnType("character varying")
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Fullname)
                    .HasColumnType("character varying")
                    .HasColumnName("fullname");

                entity.Property(e => e.Password)
                    .HasColumnType("character varying")
                    .HasColumnName("password");

                entity.Property(e => e.UserType).HasColumnName("user_type");

                entity.Property(e => e.Username)
                    .HasColumnType("character varying")
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
