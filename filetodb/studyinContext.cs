using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace importdata
{
    public partial class studyinContext : DbContext
    {
        public virtual DbSet<Abstudy> Abstudy { get; set; }
        public virtual DbSet<Count> Count { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Huji> Huji { get; set; }
        public virtual DbSet<IHistory> IHistory { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseMySql(@"Server=192.168.10.94;User Id=studyin;Password=yunyi@6688A;Database=studyin");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Abstudy>(entity =>
            {
                entity.HasKey(e => e.Idcard)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("ABSTUDY");

                entity.Property(e => e.Idcard)
                    .HasColumnName("IDCARD")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Dabh)
                    .HasColumnName("DABH")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Deductpoints)
                    .HasColumnName("DEDUCTPOINTS")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Errorcount)
                    .HasColumnName("ERRORCOUNT")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Exist)
                    .HasColumnName("EXIST")
                    .HasColumnType("varchar(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Filename)
                    .HasColumnName("FILENAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Licence)
                    .HasColumnName("LICENCE")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Licencenumber)
                    .HasColumnName("LICENCENUMBER")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Phonenumber)
                    .HasColumnName("PHONENUMBER")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Photo)
                    .HasColumnName("PHOTO")
                    .HasColumnType("varchar(2)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Sname)
                    .HasColumnName("SNAME")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Sremark)
                    .HasColumnName("SREMARK")
                    .HasColumnType("varchar(2)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Syyxqz).HasColumnName("SYYXQZ");

                entity.Property(e => e.Time).HasColumnName("TIME");
            });

            modelBuilder.Entity<Count>(entity =>
            {
                entity.HasKey(e => e.Time)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("COUNT");

                entity.Property(e => e.Time)
                    .HasColumnName("TIME")
                    .HasDefaultValueSql("0000-00-00 00:00:00");

                entity.Property(e => e.Application)
                    .HasColumnName("APPLICATION")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Applicationday)
                    .HasColumnName("APPLICATIONDAY")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Changdao)
                    .HasColumnName("CHANGDAO")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Fushanqu)
                    .HasColumnName("FUSHANQU")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Haiyang)
                    .HasColumnName("HAIYANG")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Kaifaqu)
                    .HasColumnName("KAIFAQU")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Laishanqu)
                    .HasColumnName("LAISHANQU")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Laiyang)
                    .HasColumnName("LAIYANG")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Laizhou)
                    .HasColumnName("LAIZHOU")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Longkou)
                    .HasColumnName("LONGKOU")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Mupingqu)
                    .HasColumnName("MUPINGQU")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pageview)
                    .HasColumnName("PAGEVIEW")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pageviewday)
                    .HasColumnName("PAGEVIEWDAY")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Penglai)
                    .HasColumnName("PENGLAI")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Qixia)
                    .HasColumnName("QIXIA")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zhaoyuan)
                    .HasColumnName("ZHAOYUAN")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Zhifuqu)
                    .HasColumnName("ZHIFUQU")
                    .HasColumnType("int(11)");
            });

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

            modelBuilder.Entity<Huji>(entity =>
            {
                entity.HasKey(e => e.Sfzh)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("HUJI");

                entity.Property(e => e.Sfzh)
                    .HasColumnName("SFZH")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Jiguan)
                    .HasColumnName("JIGUAN")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Minzu)
                    .HasColumnName("MINZU")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Qixian1)
                    .HasColumnName("QIXIAN1")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Qixian2)
                    .HasColumnName("QIXIAN2")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Time).HasColumnName("TIME");

                entity.Property(e => e.Zhuzhi1)
                    .HasColumnName("ZHUZHI1")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Zhuzhi2)
                    .HasColumnName("ZHUZHI2")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<IHistory>(entity =>
            {
                entity.ToTable("iHISTORY");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Address)
                    .HasColumnName("ADDRESS")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.County)
                    .HasColumnName("COUNTY")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Dabh)
                    .HasColumnName("DABH")
                    .HasColumnType("varchar(30)");

                entity.Property(e => e.Deductpoints)
                    .HasColumnName("DEDUCTPOINTS")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Failure)
                    .HasColumnName("FAILURE")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Filename)
                    .HasColumnName("FILENAME")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Idcard)
                    .HasColumnName("IDCARD")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Licence)
                    .HasColumnName("LICENCE")
                    .HasColumnType("varchar(5)");

                entity.Property(e => e.Licencenumber)
                    .HasColumnName("LICENCENUMBER")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Messaged)
                    .HasColumnName("MESSAGED")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ORDINAL")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Phonenumber)
                    .HasColumnName("PHONENUMBER")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Photo)
                    .HasColumnName("PHOTO")
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.Printed)
                    .HasColumnName("PRINTED")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Processed)
                    .HasColumnName("PROCESSED")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Sremark)
                    .HasColumnName("SREMARK")
                    .HasColumnType("varchar(2)");

                entity.Property(e => e.Status)
                    .HasColumnName("STATUS")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Studylog)
                    .HasColumnName("STUDYLOG")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Syyxqz).HasColumnName("SYYXQZ");

                entity.Property(e => e.Time).HasColumnName("TIME");

                entity.Property(e => e.Zhiduinumber)
                    .HasColumnName("ZHIDUINUMBER")
                    .HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("MESSAGE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Content)
                    .HasColumnName("CONTENT")
                    .HasColumnType("varchar(2000)");

                entity.Property(e => e.Historyid)
                    .HasColumnName("HISTORYID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Sent)
                    .HasColumnName("SENT")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Time).HasColumnName("TIME");
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