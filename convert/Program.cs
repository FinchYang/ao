using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace convert
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin!"+DateTime.Now);
            filecon1();
            Console.WriteLine("end!" + DateTime.Now);
        }
        private static string GetToken()
        {
            var seed = Guid.NewGuid().ToString("N");
            return seed;
        }
        static void dbcon1()
        {
            using (var abo = new abo.aboContext())
            {
                using (var enabo = new enabo.enaboContext())
                {

                }
            }
        }
        static void filecon1()
        {
            var sourcep = "/home/endriver/back";
            var targetp = "/home/endriver/server/pictures";
            var sd = new System.IO.DirectoryInfo(sourcep).GetDirectories();
            foreach (var id in sd)
            {               
                 var enid = CryptographyHelpers.StudyEncrypt(id.Name);
                using (var enabo = new enabo.enaboContext())
                {
                    var user = enabo.Aouser.FirstOrDefault(a => a.Identity == enid);
                    if(user==null)
                    {
                        continue;
                    }
                    var idp = Path.Combine(targetp, user.Photofile);
                    if (!System.IO.Directory.Exists(idp)) Directory.CreateDirectory(idp);
                    var idpics = new DirectoryInfo(id.FullName).GetDirectories();
                    foreach (var busi in idpics)
                    {
                        var busip = Path.Combine(idp, busi.Name);
                        if (!Directory.Exists(busip)) Directory.CreateDirectory(busip);
                        var pics = new DirectoryInfo(busi.FullName).GetFiles();
                        foreach (var pic in pics)
                        {
                            var filecontent = File.ReadAllBytes(pic.FullName);
                            var sfc = Convert.ToBase64String(filecontent);
                            var ens = CryptographyHelpers.StudyEncrypt(sfc);
                            var tfile = Path.Combine(busip, pic.Name);
                            File.WriteAllText(tfile, ens);
                        }
                    }
                }
                Console.WriteLine("id={0}", id.Name);
            }
        }
        //            static void dbcon()
        //{
        //    using(var abo=new abo.aboContext())
        //    {
        //        using(var enabo=new enabo.enaboContext())
        //        {
        //            //var abousers = abo.Aouser.Where(a => a.Identity.Length == 18);
        //            //foreach(var auser in abousers)
        //            //{
        //            //    var photofile = GetToken();
        //            //    var enid = CryptographyHelpers.StudyEncrypt(auser.Identity);
        //            //    var lastuser = enabo.Aouser.FirstOrDefault(a => a.Identity == enid);
        //            //    if (lastuser == null)
        //            //    {
        //            //        enabo.Aouser.Add(new convert.enabo.Aouser
        //            //        {
        //            //            Identity = enid,
        //            //            Photofile = photofile,
        //            //            Name = auser.Name,
        //            //            Phone = auser.Phone,
        //            //            Verificationcode = auser.Verificationcode,
        //            //            Newphone = auser.Newphone,
        //            //            Blacklist = auser.Blacklist
        //            //        });
        //            //    }
        //            //    else
        //            //    {
        //            //        lastuser.Verificationcode = auser.Verificationcode;
        //            //        lastuser.Newphone = auser.Newphone;
        //            //        lastuser.Blacklist = auser.Blacklist;
        //            //    }
        //            //    enabo.SaveChanges();
        //            //    filecon(auser.Identity, photofile);
        //            //}

        //            //var abbusiness=abo.Business.Where(a => a.Identity.Length == 18);
        //            //foreach(var busi in abbusiness)
        //            //{
        //            //    var enid = CryptographyHelpers.StudyEncrypt(busi.Identity);
        //            //    enabo.Business.Add(new convert.enabo.Business
        //            //    {
        //            //        Identity = enid,
        //            //        Businesstype=busi.Businesstype,
        //            //        Completed=busi.Completed,
        //            //        Time=busi.Time,
        //            //        Postaddr=busi.Postaddr,
        //            //        Acceptingplace=busi.Acceptingplace,
        //            //        QuasiDrivingLicense=busi.QuasiDrivingLicense,
        //            //        Status=busi.Status,
        //            //        Waittime=busi.Waittime,
        //            //        Processtime=busi.Processtime,
        //            //        Finishtime=busi.Finishtime,
        //            //        Integrated=busi.Integrated,
        //            //        Reason=busi.Reason,
        //            //        Losttime=busi.Losttime,
        //            //        Exporttime=busi.Exporttime,
        //            //        Abroadorservice=busi.Abroadorservice,
        //            //        Province=busi.Province,
        //            //        County=busi.County,
        //            //        City=busi.City,
        //            //    });
        //            //    enabo.SaveChanges();
        //            //}

        //            //Console.WriteLine("beging  businesspic");
        //            //var abpic=abo.Businesspic.Where(a => a.Identity.Length == 18);
        //            //foreach(var pic  in abpic)
        //            //{
        //            //    var enid = CryptographyHelpers.StudyEncrypt(pic.Identity);
        //            //    try
        //            //    {
        //            //        var thepic = enabo.Businesspic.FirstOrDefault(a => a.Identity == pic.Identity
        //            //         && a.Businesstype == pic.Businesstype && a.Pictype == pic.Pictype);
        //            //        if (thepic != null) continue;
        //            //        enabo.Businesspic.Add(new convert.enabo.Businesspic
        //            //        {
        //            //            Identity = enid,
        //            //            Businesstype = pic.Businesstype,
        //            //            Pictype = pic.Pictype,
        //            //            Uploaded = pic.Uploaded,
        //            //            Time = pic.Time,
        //            //        });
        //            //        enabo.SaveChanges();
        //            //    }
        //            //    catch(Exception ex) { }
        //            //}
        //            //Console.WriteLine("beging  businesspichis");
        //            //var  abpichis=abo.Businesspichis.Where(a => a.Identity.Length == 18);
        //            //foreach(var pichis in abpichis)
        //            //{
        //            //    var enid = CryptographyHelpers.StudyEncrypt(pichis.Identity);
        //            //    enabo.Businesspichis.Add(new convert.enabo.Businesspichis
        //            //    {
        //            //        Identity=enid,
        //            //        Businesstype = pichis.Businesstype,
        //            //        Pictype = pichis.Pictype,
        //            //        Uploaded = pichis.Uploaded,
        //            //        Time = pichis.Time,
        //            //    });
        //            //    enabo.SaveChanges();
        //            //}
        //            //Console.WriteLine("beging  businesshis");
        //            //var abbusihis=abo.Businesshis.Where(a => a.Identity.Length == 18);
        //            //foreach(var busihis in abbusihis)
        //            //{
        //            //    var enid = CryptographyHelpers.StudyEncrypt(busihis.Identity);
        //            //    enabo.Businesshis.Add(new convert.enabo.Businesshis
        //            //    {
        //            //        Identity=enid,
        //            //        Businesstype=busihis.Businesstype,
        //            //        Completed=busihis.Completed,
        //            //        Time=busihis.Time,
        //            //        Postaddr=busihis.Postaddr,
        //            //        Acceptingplace=busihis.Acceptingplace,
        //            //        QuasiDrivingLicense=busihis.QuasiDrivingLicense,
        //            //        Reason=busihis.Reason,

        //            //    });
        //            //    enabo.SaveChanges();
        //            }
        //        }
        //    }
        //}
        static void filecon(string oldid,string photofile)
        {
            var sourcep = "/home/endriver/server/picold";
            var targetp = "/home/endriver/server/pictures";
            var sd = new System.IO.DirectoryInfo(sourcep).GetDirectories();
            foreach(var id in sd)
            {
                if (id.Name != oldid) continue;
               // var enid = CryptographyHelpers.StudyEncrypt(id.Name);
                var idp = Path.Combine(targetp, photofile);
                if (!System.IO.Directory.Exists(idp)) Directory.CreateDirectory(idp);
                var idpics = new DirectoryInfo(id.FullName).GetDirectories();
                foreach(var busi in idpics)
                {
                    var busip = Path.Combine(idp, busi.Name);
                    if (!Directory.Exists(busip)) Directory.CreateDirectory(busip);
                    var pics = new DirectoryInfo(busi.FullName).GetFiles();
                    foreach(var pic in pics)
                    {
                        var filecontent = File.ReadAllBytes(pic.FullName);
                        var sfc = Convert.ToBase64String(filecontent);
                        var ens = CryptographyHelpers.StudyEncrypt(sfc);
                        var tfile = Path.Combine(busip, pic.Name);
                        File.WriteAllText(tfile, ens);
                    }
                }
                break;
            }
        }
    }
}
