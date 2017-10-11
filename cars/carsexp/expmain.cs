using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using carshare;
using carsexp.cars;

namespace exportdb
{
    class Program
    {
        static string dbtofilePath = "dbtofile";
        static string exportPath = "ftp/put";          
    
        static void Main(string[] args)
        {
            Console.WriteLine("{0}, export archived data to file, and zip it, started...", DateTime.Now);
            DbToFileForExtranetToIntranet();
            Console.WriteLine("{0}, export archived data to file, and zip it, completed.", DateTime.Now);
        }

        static void DbToFileForExtranetToIntranet()
        {
            var date = DateTime.Now;
            var dir = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", date.Year, date.Month.ToString("D2"), date.Day.ToString("D2"),
            date.Hour.ToString("D2"), date.Minute.ToString("D2"), date.Second.ToString("D2"));

            var dbtofilefname = "cars.txt";
            var home = "/home/carsbusiness";// Environment.GetEnvironmentVariable("HOME");
            var dbtofp = Path.Combine(home, dbtofilePath);
            if (!Directory.Exists(dbtofp)) Directory.CreateDirectory(dbtofp);
            var fname = Path.Combine(dbtofp, dbtofilefname);

            using (var db = new carsContext())
            {
                //冗余传输，防止边界平台not delivery
                var rebusi = db.Carbusiness.Where(async => async.Integrated == true
                 && async.Exporttime.CompareTo(date.AddDays(-1)) >= 0
                );
                Console.WriteLine("redundant is {1}, {2} records need to  be archived", ",", date, rebusi.Count());
                foreach (var rere in rebusi)
                {
                    var picsr = db.Carbusinesspic.Where(c => c.Businesstype == rere.Businesstype && c.Identity == rere.Identity && c.Uploaded == true);

                    if (picsr.Count() >= global.businesscount[(businessType)rere.Businesstype])
                    {
                        var aouser = db.Caruser.FirstOrDefault(aa => aa.Identity == rere.Identity);
                        if (aouser == null || string.IsNullOrEmpty(aouser.Name)) continue;

                        var bt = (businessType)rere.Businesstype;                                 

                        NewMethod(aouser.Photofile, bt.ToString(), home);
                        var addr = string.Empty;
                        if (!string.IsNullOrEmpty(rere.Province)) addr += rere.Province;
                        if (!string.IsNullOrEmpty(rere.City)) addr += rere.City;
                        if (!string.IsNullOrEmpty(rere.County)) addr += rere.County;
                        if (!string.IsNullOrEmpty(rere.Postaddr)) addr += rere.Postaddr;
                        var line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                         rere.Identity, ((businessType)rere.Businesstype).ToString(), addr, rere.Acceptingplace,
                         rere.QuasiDrivingLicense, aouser.Phone, aouser.Name, rere.Losttime.ToString("yyyy/MM/dd HH:mm:ss"),
                         rere.Abroadorservice, rere.Exporttime.ToString("yyyy/MM/dd HH:mm:ss"),aouser.Photofile);
                        File.AppendAllText(fname, line + "\r\n");
                    }
                }

                var theuser = db.Carbusiness.Where(async => async.Integrated == false);
                Console.WriteLine("today is {1}, {2} records need to  be archived", ",", date, theuser.Count());
                foreach (var re in theuser)
                {
                    var pics = db.Carbusinesspic.Where(c => c.Businesstype == re.Businesstype && c.Identity == re.Identity && c.Uploaded == true);

                    if (pics.Count() >= global.businesscount[(businessType)re.Businesstype])
                    {

                        var aouser = db.Caruser.FirstOrDefault(aa => aa.Identity == re.Identity);
                        if (aouser == null || string.IsNullOrEmpty(aouser.Name)) continue;
                        var bt = (businessType)re.Businesstype;                        

                        NewMethod(aouser.Photofile, bt.ToString(), home);
                        var addr = string.Empty;
                        if (!string.IsNullOrEmpty(re.Province)) addr += re.Province;
                        if (!string.IsNullOrEmpty(re.City)) addr += re.City;
                        if (!string.IsNullOrEmpty(re.County)) addr += re.County;
                        if (!string.IsNullOrEmpty(re.Postaddr)) addr += re.Postaddr;
                        var line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                         re.Identity, ((businessType)re.Businesstype).ToString(), addr, re.Acceptingplace,
                         re.QuasiDrivingLicense, aouser.Phone, aouser.Name, re.Losttime.ToString("yyyy/MM/dd HH:mm:ss")
                         , re.Abroadorservice, date.ToString("yyyy/MM/dd HH:mm:ss"),aouser.Photofile);
                        File.AppendAllText(fname, line + "\r\n");
                        re.Integrated = true;

                        re.Exporttime = date;
                        re.Waittime = date;
                    }
                }
                db.SaveChanges();
            }
            var exp = Path.Combine(home, exportPath);
            if (!Directory.Exists(exp)) Directory.CreateDirectory(exp);
            var zipfname = Path.Combine(exp, dir + "-abo");

            var a = new System.Diagnostics.Process();
            a.StartInfo.UseShellExecute = true;
            a.StartInfo.Arguments =
            string.Format(" -9 {0} {1}/* -r", zipfname, dbtofp);
            a.StartInfo.FileName = "zip";
            a.Start();
            a.WaitForExit();
        }
      

        private static void NewMethod(string filebase, string btype, string home)
        {
            var bpath = Path.Combine(home, dbtofilePath, filebase);
            if (!Directory.Exists(bpath)) Directory.CreateDirectory(bpath);

            var a = new System.Diagnostics.Process();
            a.StartInfo.UseShellExecute = true;
            var param = string.Format(" -r {3}/server/pictures/{1}/{2} {3}/{0}/{1}/", dbtofilePath, filebase, btype, home);
            a.StartInfo.Arguments = param;

            Console.WriteLine(param);
            a.StartInfo.FileName = "cp";
            a.Start();
            a.WaitForExit();
        }
    }
}
