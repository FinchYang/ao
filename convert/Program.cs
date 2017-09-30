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
            Console.WriteLine("Hello World!");
        }
        private static string GetToken()
        {
            var seed = Guid.NewGuid().ToString("N");
            return seed;
        }
        static void dbcon()
        {
            using(var abo=new abo.aboContext())
            {
                using(var enabo=new enabo.enaboContext())
                {
                    var abousers = abo.Aouser.Where(a => a.Identity.Length == 18);
                    foreach(var auser in abousers)
                    {
                        var photofile = GetToken();
                        enabo.Aouser.Add(new convert.enabo.Aouser
                        {
                            Identity = CryptographyHelpers.StudyEncrypt(auser.Identity),
                            Photofile = photofile,
                            Name=auser.Name,
                            Phone=auser.Phone,
                            Verificationcode=auser.Verificationcode,
                            Newphone=auser.Newphone,
                            Blacklist=auser.Blacklist
                        });
                        enabo.SaveChanges();
                        filecon(auser.Identity, photofile);
                    }
                }
            }
        }
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
