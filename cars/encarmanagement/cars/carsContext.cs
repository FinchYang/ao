using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace encm.cars
{
    public partial class carsContext : DbContext
    {
        public virtual DbSet<Carbusiness> Carbusiness { get; set; }
        public virtual DbSet<Carbusinesshis> Carbusinesshis { get; set; }
        public virtual DbSet<Carbusinesspic> Carbusinesspic { get; set; }
        public virtual DbSet<Carslog> Carslog { get; set; }
        public virtual DbSet<Caruser> Caruser { get; set; }
        public virtual DbSet<Operator> Operator { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseMySql(@"Server=192.168.10.94;User Id=studyin;Password=yunyi@6688A;Database=cars");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carbusiness>(entity =>
            {
                entity.HasKey(e => new { e.Identity, e.Businesstype })
                    .HasName("PK_carbusiness");

                entity.ToTable("carbusiness");

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Businesstype)
                    .HasColumnName("businesstype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Abroadorservice)
                    .HasColumnName("abroadorservice")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Acceptingplace)
                    .HasColumnName("acceptingplace")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Cartype)
                    .HasColumnName("cartype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.County)
                    .HasColumnName("county")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Exporttime)
                    .HasColumnName("exporttime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Finishtime)
                    .HasColumnName("finishtime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Integrated)
                    .HasColumnName("integrated")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Losttime)
                    .HasColumnName("losttime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Platenumber1)
                    .HasColumnName("platenumber1")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Platenumber2)
                    .HasColumnName("platenumber2")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Platetype)
                    .HasColumnName("platetype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Postaddr)
                    .HasColumnName("postaddr")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Processtime)
                    .HasColumnName("processtime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.QuasiDrivingLicense)
                    .HasColumnName("quasiDrivingLicense")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Scrapplace)
                    .HasColumnName("scrapplace")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.Waittime)
                    .HasColumnName("waittime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");
            });

            modelBuilder.Entity<Carbusinesshis>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("carbusinesshis");

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

                entity.Property(e => e.Cartype)
                    .HasColumnName("cartype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.County)
                    .HasColumnName("county")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Exporttime)
                    .HasColumnName("exporttime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Finishtime)
                    .HasColumnName("finishtime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Identity)
                    .IsRequired()
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Integrated)
                    .HasColumnName("integrated")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Platenumber1)
                    .HasColumnName("platenumber1")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Platenumber2)
                    .HasColumnName("platenumber2")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Platetype)
                    .HasColumnName("platetype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Postaddr)
                    .HasColumnName("postaddr")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Processtime)
                    .HasColumnName("processtime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.QuasiDrivingLicense)
                    .HasColumnName("quasiDrivingLicense")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("varchar(545)");

                entity.Property(e => e.Scrapplace)
                    .HasColumnName("scrapplace")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.Waittime)
                    .HasColumnName("waittime")
                    .HasDefaultValueSql("2000-01-01 00:00:00");
            });

            modelBuilder.Entity<Carbusinesspic>(entity =>
            {
                entity.HasKey(e => new { e.Identity, e.Businesstype, e.Pictype })
                    .HasName("PK_carbusinesspic");

                entity.ToTable("carbusinesspic");

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

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.Uploaded)
                    .HasColumnName("uploaded")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<Carslog>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("carslog");

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

                entity.Property(e => e.Time).HasColumnName("time");
            });

            modelBuilder.Entity<Caruser>(entity =>
            {
                entity.HasKey(e => e.Identity)
                    .HasName("identity_UNIQUE");

                entity.ToTable("caruser");

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Blacklist)
                    .HasColumnName("blacklist")
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("0");

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

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.ToTable("operator");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Scrapplace)
                    .HasColumnName("scrapplace")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");
            });
        }
    }
}