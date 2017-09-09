using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace exportdb
{
    class Program
    {
        static string dbtofilePath = "/home/driverbusiness/dbtofile";
        static string exportPath = "/home/driverbusiness/ftp/put";
        public enum businessType
        {
            unknown,
            ChangeLicense,//变更户籍姓名

            delay,//延期换证
            lost,//遗失补证
            damage,//损毁换证
            overage,//超龄换证

            expire,//期满换证
            changeaddr,//变更户籍地址
            basicinfo,//基本信息证明
            first,//初领、增加机动车驾驶证自愿业务退办
            network,// 网约车安全驾驶证明

            three,//三年无重大事故证明
            five,//五年安全驾驶证明
            inspectDelay//延期审验
, bodyDelay//延期提交身体证明
, changeContact//变更联系方式

        };
        public static class global
        {
            public static Dictionary<businessType, short> businesscount = new Dictionary<businessType, short>();

            static global()
            {
                businesscount.Add(businessType.unknown, 5);

                businesscount.Add(businessType.ChangeLicense, 6);//变更户籍姓名
                businesscount.Add(businessType.delay, 6);//延期换证?
                businesscount.Add(businessType.lost, 6);//遗失补证
                businesscount.Add(businessType.damage, 5);//损毁换证
                businesscount.Add(businessType.overage, 6);//超龄换证?

                businesscount.Add(businessType.expire, 6);//期满换证??6,7??
                businesscount.Add(businessType.changeaddr, 5);//变更户籍地址
                businesscount.Add(businessType.basicinfo, 4);//基本信息证明
                businesscount.Add(businessType.first, 4);//初领、增加机动车驾驶证自愿业务退办
                businesscount.Add(businessType.network, 4);// 网约车安全驾驶证明

                businesscount.Add(businessType.three, 4);//三年无重大事故证明
                businesscount.Add(businessType.five, 4);//五年安全驾驶证明

                businesscount.Add(businessType.inspectDelay, 6);//延期审验
                businesscount.Add(businessType.bodyDelay, 6);//延期提交身体证明
                businesscount.Add(businessType.changeContact, 4);//变更联系方式
            }
        }
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
            var dbtofilefname = dir + "-abo.txt";
            if (!Directory.Exists(dbtofilePath)) Directory.CreateDirectory(dbtofilePath);
            var fname = Path.Combine(dbtofilePath, dbtofilefname);
            using (var db = new blahContext())
            {
                var theuser = db.Business.Where(async => async.Integrated == false);
                Console.WriteLine("today is {1}, {2} records need to  be archived", ",", date, theuser.Count());
                foreach (var re in theuser)
                {
                    var pics = db.Businesspic.Where(c => c.Businesstype == re.Businesstype && c.Identity == re.Identity && c.Uploaded == true);

                    if (pics.Count() >= global.businesscount[(businessType)re.Businesstype])
                    {
                        NewMethod(re.Identity, re.Businesstype.ToString());
                        var phone = string.Empty;
                        if (re.Businesstype == (short)businessType.changeContact)
                        {
                            var userp=db.Aouser.FirstOrDefault(h =>h.Identity==re.Identity);
                            if(userp!=null){
                                phone=userp.Phone;
                            }
                        }
                        var line = string.Format("{0},{1},{2},{3},{4},{5}",
                         re.Identity, re.Businesstype, re.Postaddr, re.Acceptingplace, re.QuasiDrivingLicense,phone);
                        File.AppendAllText(fname, line + "\r\n");
                    }
                    re.Integrated = true;
                }
                db.SaveChanges();
            }
            if (!Directory.Exists(exportPath)) Directory.CreateDirectory(exportPath);
            var zipfname = Path.Combine(exportPath, dir+"-abo");

            var a = new System.Diagnostics.Process();
            a.StartInfo.UseShellExecute = true;
            a.StartInfo.Arguments =
            string.Format(" {0} {1}/* -r", zipfname, dbtofilePath);
            a.StartInfo.FileName = "zip";
            a.Start();
            a.WaitForExit();
        }

        private static void NewMethod(string filebase, string btype)
        {
            var a = new System.Diagnostics.Process();
            a.StartInfo.UseShellExecute = true;
            a.StartInfo.Arguments =
            string.Format(" -r /home/server/picures/{1}/{2}/* {0}/", dbtofilePath, filebase, btype);
            a.StartInfo.FileName = "cp";
            a.Start();
            a.WaitForExit();
        }
    }
}
