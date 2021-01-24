using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ShipBot.DataAccsess
{
    public partial class ShipDbContext : DbContext
    {
        public ShipDbContext()
        {
        }

        public ShipDbContext(DbContextOptions<ShipDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Ship> Ships { get; set; }
        public virtual DbSet<ShipRating> ShipRatings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlite("Data Source=c:\\Users\\dppet\\source\\repos\\ShipBot\\ShipBot.DataAccsess\\ShipDb.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasIndex(e => e.Id, "IX_Characters_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.Owner);
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.DiscordUser);
            });

            modelBuilder.Entity<Ship>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.HasOne(d => d.Character1Navigation)
                    .WithMany(p => p.ShipCharacter1Navigations)
                    .HasForeignKey(d => d.Character1);

                entity.HasOne(d => d.Character2Navigation)
                    .WithMany(p => p.ShipCharacter2Navigations)
                    .HasForeignKey(d => d.Character2);
            });

            modelBuilder.Entity<ShipRating>(entity =>
            {
                entity.HasKey(e => new { e.Rater, e.Ship });

                entity.HasOne(d => d.RaterNavigation)
                    .WithMany(p => p.ShipRatings)
                    .HasForeignKey(d => d.Rater)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ShipNavigation)
                    .WithMany(p => p.ShipRatings)
                    .HasForeignKey(d => d.Ship)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
