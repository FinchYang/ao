using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace exportdb
{
    public partial class studyinContext : DbContext
    {
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseMySql(@"Server=192.168.10.94;User Id=studyin;Password=yunyi@6688A;Database=studyin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("history");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Completelog)
                    .HasColumnName("completelog")
                    .HasColumnType("varchar(80)");

                entity.Property(e => e.Deductedmarks)
                    .HasColumnName("deductedmarks")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Drivinglicense)
                    .HasColumnName("drivinglicense")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Drugrelated)
                    .HasColumnName("drugrelated")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Finishdate).HasColumnName("finishdate");

                entity.Property(e => e.Firstsigned)
                    .HasColumnName("firstsigned")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Fullmark)
                    .HasColumnName("fullmark")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Identity)
                    .IsRequired()
                    .HasColumnName("identity")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Inspect)
                    .HasColumnName("inspect")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Lasttoken)
                    .HasColumnName("lasttoken")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Licensetype)
                    .HasColumnName("licensetype")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Noticedate).HasColumnName("noticedate");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Photofile)
                    .HasColumnName("photofile")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Photostatus)
                    .HasColumnName("photostatus")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Postaladdress)
                    .HasColumnName("postaladdress")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Signed)
                    .HasColumnName("signed")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Startdate).HasColumnName("startdate");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Stoplicense)
                    .HasColumnName("stoplicense")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Studylog)
                    .HasColumnName("studylog")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Syncdate).HasColumnName("syncdate");

                entity.Property(e => e.Syncphone)
                    .HasColumnName("syncphone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Wechat)
                    .HasColumnName("wechat")
                    .HasColumnType("varchar(45)");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("request");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasColumnType("int(11)");

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Identity)
                    .HasName("identity_UNIQUE");

                entity.ToTable("user");

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Completelog)
                    .HasColumnName("completelog")
                    .HasColumnType("varchar(80)");

                entity.Property(e => e.Deductedmarks)
                    .HasColumnName("deductedmarks")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Drivinglicense)
                    .HasColumnName("drivinglicense")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Drugrelated)
                    .HasColumnName("drugrelated")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Firstsigned)
                    .HasColumnName("firstsigned")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Fullmark)
                    .HasColumnName("fullmark")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Inspect)
                    .HasColumnName("inspect")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Lasttoken)
                    .HasColumnName("lasttoken")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Licensetype)
                    .HasColumnName("licensetype")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Noticedate).HasColumnName("noticedate");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Photofile)
                    .HasColumnName("photofile")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Photostatus)
                    .HasColumnName("photostatus")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Postaladdress)
                    .HasColumnName("postaladdress")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Signed)
                    .HasColumnName("signed")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Startdate).HasColumnName("startdate");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Stoplicense)
                    .HasColumnName("stoplicense")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Studylog)
                    .HasColumnName("studylog")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Syncdate).HasColumnName("syncdate");

                entity.Property(e => e.Syncphone)
                    .HasColumnName("syncphone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Wechat)
                    .HasColumnName("wechat")
                    .HasColumnType("varchar(45)");
            });
        }
    }
}