using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EF_Core_DbFirst_Demo.Models
{
    public partial class vanfjContext : DbContext
    {
        public virtual DbSet<Userentity> Userentity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;User Id=root;Password=123456;Database=vanfj");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Userentity>(entity =>
            {
                entity.ToTable("userentity");

                entity.Property(e => e.Id).HasColumnType("int(10)");

                entity.Property(e => e.Age).HasColumnType("int(10)");

                entity.Property(e => e.Phone).HasMaxLength(255);

                entity.Property(e => e.Qq)
                    .HasColumnName("QQ")
                    .HasMaxLength(255);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(255);
            });
        }
    }
}
