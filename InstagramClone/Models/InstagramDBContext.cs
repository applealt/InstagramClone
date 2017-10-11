using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InstagramClone.Models
{
    public partial class InstagramDBContext : DbContext
    {
        public virtual DbSet<Follows> Follows { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }
        public virtual DbSet<PasswordRecoveries> PasswordRecoveries { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public InstagramDBContext(DbContextOptions<InstagramDBContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=InstagramDB;User Id=sa;Password=D@n13lD@ng28;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follows>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FollowDate)
                    .HasColumnName("followDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FollowedUser).HasColumnName("followedUser");

                entity.Property(e => e.FollowingUser).HasColumnName("followingUser");

                entity.HasOne(d => d.FollowedUserNavigation)
                    .WithMany(p => p.FollowsFollowedUserNavigation)
                    .HasForeignKey(d => d.FollowedUser)
                    .HasConstraintName("FK__Follows__followe__47DBAE45");

                entity.HasOne(d => d.FollowingUserNavigation)
                    .WithMany(p => p.FollowsFollowingUserNavigation)
                    .HasForeignKey(d => d.FollowingUser)
                    .HasConstraintName("FK__Follows__followi__46E78A0C");
            });

            modelBuilder.Entity<Likes>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LikeDate)
                    .HasColumnName("likeDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Post).HasColumnName("post");

                entity.Property(e => e.User).HasColumnName("user");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.Post)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Likes__post__4316F928");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Likes__user__4222D4EF");
            });

            modelBuilder.Entity<PasswordRecoveries>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnName("expirationDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.User).HasColumnName("user");

                entity.Property(e => e.VerifyId)
                    .IsRequired()
                    .HasColumnName("verifyID")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.PasswordRecoveries)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PasswordRe__user__4BAC3F29");
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.FeedbackCount)
                    .HasColumnName("feedbackCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HashTag)
                    .HasColumnName("hashTag")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnName("image")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LikeCount)
                    .HasColumnName("likeCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.User).HasColumnName("user");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Posts__user__3D5E1FD2");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasColumnName("displayName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FollowersCount)
                    .HasColumnName("followersCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FollowingCount)
                    .HasColumnName("followingCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ImageProfile)
                    .HasColumnName("imageProfile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PostsCount)
                    .HasColumnName("postsCount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
