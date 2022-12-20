using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IMS.Entities
{
    public partial class IMSDBContext : DbContext
    {
        public IMSDBContext(DbContextOptions<IMSDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MasterBarang> MasterBarangs { get; set; }
        public virtual DbSet<MasterGudang> MasterGudangs { get; set; }
        public virtual DbSet<MasterKategori> MasterKategoris { get; set; }
        public virtual DbSet<MasterUser> MasterUsers { get; set; }
        public virtual DbSet<Outlet> Outlets { get; set; }
        public virtual DbSet<StockTransaction> StockTransactions { get; set; }
        public virtual DbSet<UserRoleEnum> UserRoleEnums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=ftp.leebongisland.com;Port=3306;Database=ariesch_pbidn11111;Uid=ariesch_pbidn11111;Pwd=fGSGVFZFmV22wy@;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasterBarang>(entity =>
            {
                entity.HasKey(e => e.SKUID)
                    .HasName("PRIMARY");

                entity.ToTable("MasterBarang");

                entity.HasIndex(e => e.KategoriCode, "KategoriCode");

                entity.Property(e => e.SKUID).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.Property(e => e.KategoriCode)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.HasOne(d => d.KategoriCodeNavigation)
                    .WithMany(p => p.MasterBarangs)
                    .HasForeignKey(d => d.KategoriCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MasterBarang_ibfk_1");
            });

            modelBuilder.Entity<MasterGudang>(entity =>
            {
                entity.HasKey(e => e.GudangCode)
                    .HasName("PRIMARY");

                entity.ToTable("MasterGudang");

                entity.HasIndex(e => e.OutletCode, "OutletCode");

                entity.Property(e => e.GudangCode).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OutletCode)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.HasOne(d => d.OutletCodeNavigation)
                    .WithMany(p => p.MasterGudangs)
                    .HasForeignKey(d => d.OutletCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MasterGudang_ibfk_1");
            });

            modelBuilder.Entity<MasterKategori>(entity =>
            {
                entity.HasKey(e => e.KategoriCode)
                    .HasName("PRIMARY");

                entity.ToTable("MasterKategori");

                entity.Property(e => e.KategoriCode).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");
            });

            modelBuilder.Entity<MasterUser>(entity =>
            {
                entity.HasKey(e => e.UserCode)
                    .HasName("PRIMARY");

                entity.ToTable("MasterUser");

                entity.HasIndex(e => e.UserRoleEnumId, "UserRoleEnumId");

                entity.Property(e => e.UserCode).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.Property(e => e.UserRoleEnumId).HasColumnType("int(11)");

                entity.HasOne(d => d.UserRoleEnum)
                    .WithMany(p => p.MasterUsers)
                    .HasForeignKey(d => d.UserRoleEnumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MasterUser_ibfk_1");
            });

            modelBuilder.Entity<Outlet>(entity =>
            {
                entity.HasKey(e => e.OutletCode)
                    .HasName("PRIMARY");

                entity.ToTable("Outlet");

                entity.Property(e => e.OutletCode).HasMaxLength(255);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");
            });

            modelBuilder.Entity<StockTransaction>(entity =>
            {
                entity.ToTable("StockTransaction");

                entity.HasIndex(e => e.GudangCode, "GudangCode");

                entity.HasIndex(e => e.SKUID, "SKUID");

                entity.Property(e => e.StockTransactionId).HasColumnType("int(11)");

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.Property(e => e.GudangCode)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SKUID)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StockAfter).HasColumnType("int(11)");

                entity.Property(e => e.StockBefore).HasColumnType("int(11)");

                entity.Property(e => e.StockIn)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.StockOut)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("'current_timestamp()'");

                entity.Property(e => e.UpdatedBy)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasDefaultValueSql("'''SYSTEM'''");

                entity.HasOne(d => d.GudangCodeNavigation)
                    .WithMany(p => p.StockTransactions)
                    .HasForeignKey(d => d.GudangCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StockTransaction_ibfk_2");

                entity.HasOne(d => d.SKU)
                    .WithMany(p => p.StockTransactions)
                    .HasForeignKey(d => d.SKUID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("StockTransaction_ibfk_1");
            });

            modelBuilder.Entity<UserRoleEnum>(entity =>
            {
                entity.ToTable("UserRoleEnum");

                entity.Property(e => e.UserRoleEnumId).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
