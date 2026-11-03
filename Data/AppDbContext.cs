using Microsoft.EntityFrameworkCore;
using TranslationDB.Models;

namespace TranslationDB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Paragraph> Paragraphs { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum conversions to string
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Paragraph>()
                .Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Translation>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<Review>()
                .Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            modelBuilder.Entity<ProjectAssignment>()
                .Property(pa => pa.AssignmentType)
                .HasConversion<string>()
                .HasMaxLength(20);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.UserName).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                
                entity.Property(u => u.Id).HasMaxLength(450);
                entity.Property(u => u.UserName).HasMaxLength(255).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
                entity.Property(u => u.FullName).HasMaxLength(255).IsRequired();
                entity.Property(u => u.ProfileImageUrl).HasMaxLength(500);
                entity.Property(u => u.CreatedAt).HasColumnType("datetime(6)");
                entity.Property(u => u.LastLoginAt).HasColumnType("datetime(6)");
            });

            // Project configurations
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.CreatedById);

                entity.Property(p => p.Name).HasMaxLength(255).IsRequired();
                entity.Property(p => p.SourceLanguage).HasMaxLength(10).IsRequired();
                entity.Property(p => p.CreatedBy).HasMaxLength(450).IsRequired();
                entity.Property(p => p.CreatedAt).HasColumnType("datetime(6)");
                entity.Property(p => p.UpdatedAt).HasColumnType("datetime(6)");
                entity.Property(p => p.Deadline).HasColumnType("datetime(6)");

                // Relationships
                entity.HasOne(p => p.Creator)
                      .WithMany(u => u.CreatedProjects)
                      .HasForeignKey(p => p.CreatedBy)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Paragraph configurations
            modelBuilder.Entity<Paragraph>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.ProjectId);
                entity.HasIndex(p => p.Status);

                entity.Property(p => p.LockedBy).HasMaxLength(450);
                entity.Property(p => p.CreatedAt).HasColumnType("datetime(6)");
                entity.Property(p => p.UpdatedAt).HasColumnType("datetime(6)");
                entity.Property(p => p.LockedAt).HasColumnType("datetime(6)");

                // Relationships
                entity.HasOne(p => p.Project)
                      .WithMany(pr => pr.Paragraphs)
                      .HasForeignKey(p => p.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.LockedByUser)
                      .WithMany(u => u.LockedParagraphs)
                      .HasForeignKey(p => p.LockedBy)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Translation configurations
            modelBuilder.Entity<Translation>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasIndex(t => t.ParagraphId);
                entity.HasIndex(t => t.TranslatorId);

                entity.Property(t => t.Id).HasMaxLength(450);
                entity.Property(t => t.TargetLanguage).HasMaxLength(10).IsRequired();
                entity.Property(t => t.TranslatorId).HasMaxLength(450).IsRequired();
                entity.Property(t => t.TranslatedAt).HasColumnType("datetime(6)");
                entity.Property(t => t.SubmittedAt).HasColumnType("datetime(6)");

                // Relationships
                entity.HasOne(t => t.Paragraph)
                      .WithMany(p => p.Translations)
                      .HasForeignKey(t => t.ParagraphId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Translator)
                      .WithMany(u => u.Translations)
                      .HasForeignKey(t => t.TranslatorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Review configurations
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.TranslationId);
                entity.HasIndex(r => r.ReviewerId);

                entity.Property(r => r.Id).HasMaxLength(450);
                entity.Property(r => r.TranslationId).HasMaxLength(450).IsRequired();
                entity.Property(r => r.ReviewerId).HasMaxLength(450).IsRequired();
                entity.Property(r => r.ReviewedAt).HasColumnType("datetime(6)");

                // Check constraint for Rating
                entity.HasCheckConstraint("CK_Reviews_Rating", "Rating IS NULL OR (Rating BETWEEN 1 AND 5)");

                // Relationships
                entity.HasOne(r => r.Translation)
                      .WithMany(t => t.Reviews)
                      .HasForeignKey(r => r.TranslationId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Reviewer)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.ReviewerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ProjectAssignment configurations
            modelBuilder.Entity<ProjectAssignment>(entity =>
            {
                entity.HasKey(pa => pa.Id);
                entity.HasIndex(pa => pa.ProjectId);
                entity.HasIndex(pa => pa.UserId);

                entity.Property(pa => pa.Id).HasMaxLength(450);
                entity.Property(pa => pa.ProjectId).IsRequired();
                entity.Property(pa => pa.UserId).HasMaxLength(450).IsRequired();
                entity.Property(pa => pa.AssignedAt).HasColumnType("datetime(6)");

                // Relationships
                entity.HasOne(pa => pa.Project)
                      .WithMany(p => p.ProjectAssignments)
                      .HasForeignKey(pa => pa.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pa => pa.User)
                      .WithMany(u => u.ProjectAssignments)
                      .HasForeignKey(pa => pa.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Notification configurations
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.HasIndex(n => new { n.UserId, n.IsRead });

                entity.Property(n => n.Id).HasMaxLength(450);
                entity.Property(n => n.UserId).HasMaxLength(450).IsRequired();
                entity.Property(n => n.Type).HasMaxLength(50).IsRequired();
                entity.Property(n => n.Title).HasMaxLength(255).IsRequired();
                entity.Property(n => n.RelatedEntityId).HasMaxLength(450);
                entity.Property(n => n.CreatedAt).HasColumnType("datetime(6)");
                entity.Property(n => n.ReadAt).HasColumnType("datetime(6)");

                // Relationships
                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically update UpdatedAt for entities that have it
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Project && 
                           (e.State == EntityState.Modified || e.State == EntityState.Added));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    ((Project)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}