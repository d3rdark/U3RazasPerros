﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace U3RazasPerros.Models
{
    public partial class perrosContext : DbContext
    {
        public perrosContext()
        {
        }

        public perrosContext(DbContextOptions<perrosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Caracteristicasfisicas> Caracteristicasfisicas { get; set; }
        public virtual DbSet<Paises> Paises { get; set; }
        public virtual DbSet<Razas> Razas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {        
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Caracteristicasfisicas>(entity =>
            {
                entity.ToTable("caracteristicasfisicas");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Cola).HasMaxLength(500);

                entity.Property(e => e.Color).HasMaxLength(500);

                entity.Property(e => e.Hocico).HasMaxLength(500);

                entity.Property(e => e.Patas).HasMaxLength(500);

                entity.Property(e => e.Pelo).HasMaxLength(500);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Caracteristicasfisicas)
                    .HasForeignKey<Caracteristicasfisicas>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_caracteristicasfisicas_1");
            });

            modelBuilder.Entity<Paises>(entity =>
            {
                entity.ToTable("paises");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.Property(e => e.Nombre).HasMaxLength(45);
            });

            modelBuilder.Entity<Razas>(entity =>
            {
                entity.ToTable("razas");

                entity.HasCharSet("latin1")
                    .UseCollation("latin1_swedish_ci");

                entity.HasIndex(e => e.IdPais, "pi_idx");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OtrosNombres).HasMaxLength(500);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Razas)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fkpai");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
