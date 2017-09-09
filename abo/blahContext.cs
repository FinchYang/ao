using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mvc104
{
    public partial class blahContext : DbContext
    {
        public virtual DbSet<Abstudy> Abstudy { get; set; }
        public virtual DbSet<Aouser> Aouser { get; set; }
        public virtual DbSet<Blahuser> Blahuser { get; set; }
        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<Businesshis> Businesshis { get; set; }
        public virtual DbSet<Businesspic> Businesspic { get; set; }
        public virtual DbSet<Businesspichis> Businesspichis { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseMySql(@"Server=47.93.226.74;User Id=blah;Password=ycl1mail@A;Database=blah");
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

                entity.Property(e => e.Syyxqz)
                    .HasColumnName("SYYXQZ")
                    .HasColumnType("datetime");

                entity.Property(e => e.Time)
                    .HasColumnName("TIME")
                    .HasColumnType("datetime");
            });

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

            modelBuilder.Entity<Blahuser>(entity =>
            {
                entity.HasKey(e => e.Ordinal)
                    .HasName("ordinal_UNIQUE");

                entity.ToTable("blahuser");

                entity.HasIndex(e => e.Loginaccount)
                    .HasName("loginaccount_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Ordinal)
                    .HasColumnName("ordinal")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Identity)
                    .HasColumnName("identity")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Loginaccount)
                    .IsRequired()
                    .HasColumnName("loginaccount")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Notation)
                    .HasColumnName("notation")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Wechat)
                    .HasColumnName("wechat")
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

                entity.Property(e => e.Finishdate)
                    .HasColumnName("finishdate")
                    .HasColumnType("datetime");

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

                entity.Property(e => e.Noticedate)
                    .HasColumnName("noticedate")
                    .HasColumnType("datetime");

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

                entity.Property(e => e.Startdate)
                    .HasColumnName("startdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Stoplicense)
                    .HasColumnName("stoplicense")
                    .HasColumnType("varchar(1)");

                entity.Property(e => e.Studylog)
                    .HasColumnName("studylog")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Syncdate)
                    .HasColumnName("syncdate")
                    .HasColumnType("datetime");

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

                entity.Property(e => e.Time)
                    .HasColumnName("TIME")
                    .HasColumnType("datetime");
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

                entity.Property(e => e.DelayPic)
                    .HasColumnName("delay_pic")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Driver)
                    .HasColumnName("driver")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Expire)
                    .HasColumnName("expire")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Health)
                    .HasColumnName("health")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.HukouPic)
                    .HasColumnName("hukou_pic")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IdBack)
                    .HasColumnName("id_back")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IdFront)
                    .HasColumnName("id_front")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IdInhand)
                    .HasColumnName("id_inhand")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Lasttoken)
                    .HasColumnName("lasttoken")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Overage)
                    .HasColumnName("overage")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Photofile)
                    .HasColumnName("photofile")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Postaddr)
                    .HasColumnName("postaddr")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.Signed)
                    .HasColumnName("signed")
                    .HasColumnType("varchar(1)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Syncdate)
                    .HasColumnName("syncdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Syncphone)
                    .HasColumnName("syncphone")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasColumnType("varchar(45)");
            });
        }
    }
}