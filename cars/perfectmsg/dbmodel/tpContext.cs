using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace perfectmsg.dbmodel
{
    public partial class tpContext : DbContext
    {
        public virtual DbSet<Dataitem> Dataitem { get; set; }
        public virtual DbSet<Reportlog> Reportlog { get; set; }
        public virtual DbSet<Summarized> Summarized { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Userlog> Userlog { get; set; }
        public virtual DbSet<Videoreport> Videoreport { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseMySql(@"Server=47.93.226.74;User Id=blah;Password=ycl1mail@A;Database=tp");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dataitem>(entity =>
            {
                entity.ToTable("dataitem");

                entity.HasIndex(e => e.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Centerdisplay)
                    .HasColumnName("centerdisplay")
                    .HasColumnType("smallint(1)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(146)");

                entity.Property(e => e.Datatype)
                    .HasColumnName("datatype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("smallint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Inputtype)
                    .HasColumnName("inputtype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("2");

                entity.Property(e => e.Mandated)
                    .HasColumnName("mandated")
                    .HasColumnType("smallint(1)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Seconditem)
                    .HasColumnName("seconditem")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Unitdisplay)
                    .HasColumnName("unitdisplay")
                    .HasColumnType("smallint(1)")
                    .HasDefaultValueSql("1");
            });

            modelBuilder.Entity<Reportlog>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.Unitid })
                    .HasName("PK_reportlog");

                entity.ToTable("reportlog");

                entity.HasIndex(e => e.Date)
                    .HasName("date_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Unitid)
                    .HasName("reportlogunitid_idx");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Unitid)
                    .HasColumnName("unitid")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("varchar(4500)");

                entity.Property(e => e.Declinereason)
                    .HasColumnName("declinereason")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Draft)
                    .HasColumnName("draft")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Reportlog)
                    .HasForeignKey(d => d.Unitid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("reportlogunitid");
            });

            modelBuilder.Entity<Summarized>(entity =>
            {
                entity.HasKey(e => e.Date)
                    .HasName("date_UNIQUE");

                entity.ToTable("summarized");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("varchar(4500)");

                entity.Property(e => e.Draft)
                    .HasColumnName("draft")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.ToTable("template");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.File)
                    .IsRequired()
                    .HasColumnName("file")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(145)");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnName("path")
                    .HasColumnType("varchar(450)");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("unit");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("ip")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasColumnType("smallint(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(145)");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Unitid)
                    .HasName("unitid_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasColumnName("pass")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Unitid)
                    .IsRequired()
                    .HasColumnName("unitid")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.Unitid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("unitid");
            });

            modelBuilder.Entity<Userlog>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("userlog");

                entity.HasIndex(e => e.Userid)
                    .HasName("userid_idx");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasColumnName("userid")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userlog)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("userid");
            });

            modelBuilder.Entity<Videoreport>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.Unitid })
                    .HasName("PK_videoreport");

                entity.ToTable("videoreport");

                entity.HasIndex(e => e.Date)
                    .HasName("date_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Unitid)
                    .HasName("reportlogunitid_idx");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Unitid)
                    .HasColumnName("unitid")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("varchar(4500)");

                entity.Property(e => e.Declinereason)
                    .HasColumnName("declinereason")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Draft)
                    .HasColumnName("draft")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Signtype)
                    .HasColumnName("signtype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Videoreport)
                    .HasForeignKey(d => d.Unitid)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("videoreportunitid");
            });
        }
    }
}