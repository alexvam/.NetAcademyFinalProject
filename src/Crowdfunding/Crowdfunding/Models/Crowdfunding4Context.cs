using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Crowdfunding.Models
{
    public partial class Crowdfunding4Context : DbContext
    {
        public Crowdfunding4Context()
        {
        }

        public Crowdfunding4Context(DbContextOptions<Crowdfunding4Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialType> MaterialType { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectCategory> ProjectCategory { get; set; }
        public virtual DbSet<ProjectStatus> ProjectStatus { get; set; }
        public virtual DbSet<Reward> Reward { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<Update> Update { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-9T1C7LJ\\SQLEXPRESS;Database=Crowdfunding4;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.Comment1)
                    .IsRequired()
                    .HasColumnName("Comment")
                    .HasColumnType("text");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Member");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Project");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.MaterialId)
                    .HasColumnName("MaterialID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Link).HasColumnType("text");

                entity.Property(e => e.MaterialTypeId).HasColumnName("MaterialTypeID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.MaterialType)
                    .WithMany(p => p.Material)
                    .HasForeignKey(d => d.MaterialTypeId)
                    .HasConstraintName("FK_Material_MaterialType");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Material)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Material_Project");
            });

            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.Property(e => e.MaterialTypeId)
                    .HasColumnName("MaterialTypeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.TypeName).HasMaxLength(250);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Address).HasMaxLength(30);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(30);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PhoneNumber).HasMaxLength(30);

                entity.Property(e => e.PostCode).HasMaxLength(30);
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.PackagesId);

                entity.Property(e => e.PackagesId)
                    .HasColumnName("PackagesID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.RewardsId).HasColumnName("RewardsID");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Package)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Package_Project");

                entity.HasOne(d => d.Rewards)
                    .WithMany(p => p.Package)
                    .HasForeignKey(d => d.RewardsId)
                    .HasConstraintName("FK_Package_Reward");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectCategoryId).HasColumnName("ProjectCategoryID");

                entity.Property(e => e.ProjectDescription).HasMaxLength(250);

                entity.Property(e => e.ProjectLocation).HasMaxLength(50);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Target).HasColumnType("decimal(14, 2)");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Member");

                entity.HasOne(d => d.ProjectCategory)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.ProjectCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_ProjectCategory");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_ProjectStatus");
            });

            modelBuilder.Entity<ProjectCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<ProjectStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusCategory)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StatusDescription).HasMaxLength(250);
            });

            modelBuilder.Entity<Reward>(entity =>
            {
                entity.HasKey(e => e.RewardsId);

                entity.Property(e => e.RewardsId)
                    .HasColumnName("RewardsID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.ItemsIncluded).HasMaxLength(250);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Reward)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reward_Project");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Contribution).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PackagesId).HasColumnName("PackagesID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Member");

                entity.HasOne(d => d.Packages)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.PackagesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Package");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Project");
            });

            modelBuilder.Entity<Update>(entity =>
            {
                entity.Property(e => e.UpdateId).HasColumnName("UpdateID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.UpdateText)
                    .IsRequired()
                    .HasColumnName("Update_Text")
                    .HasColumnType("text");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Update)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Update_Member");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Update)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Update_Project");
            });
        }
    }
}
