using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace perfectmsg.aadb
{
    public partial class aaContext : DbContext
    {
        public virtual DbSet<Dataitem> Dataitem { get; set; }
        public virtual DbSet<Moban> Moban { get; set; }
        public virtual DbSet<Reportlog> Reportlog { get; set; }
        public virtual DbSet<Reports> Reports { get; set; }
        public virtual DbSet<Reportsdata> Reportsdata { get; set; }
        public virtual DbSet<Summarized> Summarized { get; set; }
        public virtual DbSet<Unit> Unit { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Userlog> Userlog { get; set; }
        public virtual DbSet<Weeksummarized> Weeksummarized { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseMySql(@"Server=localhost;port=5678;User Id=root;Password=root5678;Database=aa");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dataitem>(entity =>
            {
                entity.ToTable("dataitem");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Defaultvalue)
                    .HasColumnName("defaultvalue")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Hassecond)
                    .HasColumnName("hassecond")
                    .HasColumnType("smallint(2)");

                entity.Property(e => e.Index)
                    .HasColumnName("index")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Inputtype)
                    .HasColumnName("inputtype")
                    .HasColumnType("smallint(2)");

                entity.Property(e => e.Mandated)
                    .HasColumnName("mandated")
                    .HasColumnType("smallint(2)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Seconditem)
                    .HasColumnName("seconditem")
                    .HasColumnType("varchar(5000)");

                entity.Property(e => e.Statisticstype)
                    .IsRequired()
                    .HasColumnName("statisticstype")
                    .HasColumnType("varchar(600)");

                entity.Property(e => e.Tabletype)
                    .IsRequired()
                    .HasColumnName("tabletype")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Units)
                    .IsRequired()
                    .HasColumnName("units")
                    .HasColumnType("varchar(300)");
            });

            modelBuilder.Entity<Moban>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("name_UNIQUE");

                entity.ToTable("moban");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(150)");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnName("comment")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Deleted)
                    .HasColumnName("deleted")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasColumnName("filename")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Tabletype)
                    .IsRequired()
                    .HasColumnName("tabletype")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
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

            modelBuilder.Entity<Reports>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("name_UNIQUE");

                entity.ToTable("reports");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(600)");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Units)
                    .IsRequired()
                    .HasColumnName("units")
                    .HasColumnType("varchar(600)");
            });

            modelBuilder.Entity<Reportsdata>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.Unitid, e.Rname })
                    .HasName("PK_reportsdata");

                entity.ToTable("reportsdata");

                entity.HasIndex(e => e.Unitid)
                    .HasName("reportsdataunitid_idx");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Unitid)
                    .HasColumnName("unitid")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Rname)
                    .HasColumnName("rname")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("varchar(20000)");

                entity.Property(e => e.Declinereason)
                    .HasColumnName("declinereason")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Draft)
                    .HasColumnName("draft")
                    .HasColumnType("smallint(2)");

                entity.Property(e => e.Signtype)
                    .HasColumnName("signtype")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Submittime)
                    .HasColumnName("submittime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Summarized>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.Reportname })
                    .HasName("PK_summarized");

                entity.ToTable("summarized");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Reportname)
                    .HasColumnName("reportname")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("varchar(21000)");

                entity.Property(e => e.Draft)
                    .HasColumnName("draft")
                    .HasColumnType("smallint(2)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
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
                    .HasColumnType("smallint(1)");

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

                entity.Property(e => e.Disabled)
                    .HasColumnName("disabled")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasColumnType("smallint(2)")
                    .HasDefaultValueSql("1");

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

                entity.Property(e => e.Unitclass)
                    .HasColumnName("unitclass")
                    .HasColumnType("smallint(2)");

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

            modelBuilder.Entity<Weeksummarized>(entity =>
            {
                entity.HasKey(e => new { e.Startdate, e.Enddate, e.Reportname })
                    .HasName("PK_weeksummarized");

                entity.ToTable("weeksummarized");

                entity.Property(e => e.Startdate)
                    .HasColumnName("startdate")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Enddate)
                    .HasColumnName("enddate")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Reportname)
                    .HasColumnName("reportname")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasColumnType("varchar(450)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasColumnType("varchar(21000)");

                entity.Property(e => e.Draft)
                    .HasColumnName("draft")
                    .HasColumnType("smallint(2)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
            });
        }
    }
}