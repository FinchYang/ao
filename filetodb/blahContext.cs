using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace importdata
{
    public partial class blahContext : DbContext
    {
        public virtual DbSet<Aouser> Aouser { get; set; }
        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<Businesshis> Businesshis { get; set; }
        public virtual DbSet<Businesspic> Businesspic { get; set; }
        public virtual DbSet<Businesspichis> Businesspichis { get; set; }
        public virtual DbSet<Request> Request { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseMySql(@"Server=47.93.226.74;User Id=blah;Password=ycl1mail@A;Database=blah");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aouser>(entity =>
            {
                entity.HasKey(e => e.Identity)
                    .HasName("identity_UNIQUE");

                entity.ToTable("aouser");

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Newphone)
                    .HasColumnName("newphone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Photofile)
                    .HasColumnName("photofile")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Verificationcode)
                    .HasColumnName("verificationcode")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => new { e.Identity, e.Businesstype })
                    .HasName("PK_business");

                entity.ToTable("business");

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Businesstype)
                    .HasColumnName("businesstype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Acceptingplace)
                    .HasColumnName("acceptingplace")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Finishtime)
                    .HasColumnName("finishtime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Integrated)
                    .HasColumnName("integrated")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Postaddr)
                    .HasColumnName("postaddr")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Processtime)
                    .HasColumnName("processtime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.QuasiDrivingLicense)
                    .HasColumnName("quasiDrivingLicense")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Waittime)
                    .HasColumnName("waittime")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");
            });

            modelBuilder.Entity<Businesshis>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("businesshis");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Acceptingplace)
                    .HasColumnName("acceptingplace")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Businesstype)
                    .HasColumnName("businesstype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Identity)
                    .IsRequired()
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Postaddr)
                    .HasColumnName("postaddr")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.QuasiDrivingLicense)
                    .HasColumnName("quasiDrivingLicense")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Businesspic>(entity =>
            {
                entity.HasKey(e => new { e.Identity, e.Businesstype, e.Pictype })
                    .HasName("PK_businesspic");

                entity.ToTable("businesspic");

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Businesstype)
                    .HasColumnName("businesstype")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Pictype)
                    .HasColumnName("pictype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Uploaded)
                    .HasColumnName("uploaded")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Businesspichis>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("businesspichis");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Businesstype)
                    .HasColumnName("businesstype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Identity)
                    .IsRequired()
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Pictype)
                    .HasColumnName("pictype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Uploaded)
                    .HasColumnName("uploaded")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("request");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Businesstype)
                    .HasColumnName("businesstype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("varchar(4500)");

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Method)
                    .HasColumnName("method")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
            });
        }
    }
}