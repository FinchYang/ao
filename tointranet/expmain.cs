using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace exportdb
{
    class Program
    {
        static string dbtofilePath = "dbtofile";
        static string exportPath = "ftp/put";
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

        public enum picType
        {
            unknown,
            id_front,//身份证正面
            id_back,//身份证反面
            id_inhand, delay_pic, driver,

            health,//机动车驾驶人身体条件证明
            overage, expire,
            hukou_pic, //户口簿本人信息变更页
            sign_pic, //驾驶证遗失声明
            declaration_sign,//申告义务签名

            face1,//人脸识别拍照一
            face2,//人脸识别拍照二
            face3,
            passport//护照
      , visa//签证
      , serviceNote//入伍通知书
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
                businesscount.Add(businessType.overage, 6);//超龄换证?67

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
            var dbtofilefname = "abo.txt";
            var home = "/home/driverbusiness";// Environment.GetEnvironmentVariable("HOME");
            var dbtofp = Path.Combine(home, dbtofilePath);
            if (!Directory.Exists(dbtofp)) Directory.CreateDirectory(dbtofp);
            var fname = Path.Combine(dbtofp, dbtofilefname);

            using (var db = new aboContext())
            {
                //冗余传输，防止边界平台not delivery
                var rebusi = db.Business.Where(async => async.Integrated == true 
               // && async.Exporttime.CompareTo(date.AddDays(-1)) >= 0
                );
                Console.WriteLine("redundant is {1}, {2} records need to  be archived", ",", date, rebusi.Count());
                foreach (var rere in rebusi)
                {
                    var picsr = db.Businesspic.Where(c => c.Businesstype == rere.Businesstype && c.Identity == rere.Identity && c.Uploaded == true);

                    if (picsr.Count() >= global.businesscount[(businessType)rere.Businesstype])
                    {
                        var bt = (businessType)rere.Businesstype;
                        if (bt == businessType.delay || bt == businessType.overage || bt == businessType.expire || bt == businessType.bodyDelay)
                        {
                            if (!checkSignpic(bt, rere.Identity, home)) continue;
                        }

                        var aouser = db.Aouser.FirstOrDefault(aa => aa.Identity == rere.Identity);
                        if (aouser == null || string.IsNullOrEmpty(aouser.Name)) continue;

                        NewMethod(rere.Identity, bt.ToString(), home);
                        // var phone = string.Empty;

                        // var userp = db.Aouser.FirstOrDefault(h => h.Identity == rere.Identity);
                        // if (userp != null)
                        // {
                        //     phone = userp.Phone;
                        // }

                        var line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                         rere.Identity, ((businessType)rere.Businesstype).ToString(), rere.Postaddr, rere.Acceptingplace,
                         rere.QuasiDrivingLicense,aouser.Phone , aouser.Name, rere.Losttime,rere.Abroadorservice,rere.Exporttime);
                        File.AppendAllText(fname, line + "\r\n");
                    }
                }

                var theuser = db.Business.Where(async => async.Integrated == false);
                Console.WriteLine("today is {1}, {2} records need to  be archived", ",", date, theuser.Count());
                foreach (var re in theuser)
                {
                    var pics = db.Businesspic.Where(c => c.Businesstype == re.Businesstype && c.Identity == re.Identity && c.Uploaded == true);

                    if (pics.Count() >= global.businesscount[(businessType)re.Businesstype])
                    {
                        var bt = (businessType)re.Businesstype;
                        if (bt == businessType.delay || bt == businessType.overage || bt == businessType.expire || bt == businessType.bodyDelay)
                        {
                            if (!checkSignpic(bt, re.Identity, home)) continue;
                        }

                        var aouser = db.Aouser.FirstOrDefault(aa => aa.Identity == re.Identity);
                        if (aouser == null || string.IsNullOrEmpty(aouser.Name)) continue;

                        NewMethod(re.Identity, bt.ToString(), home);
                        // var phone = string.Empty;
                        // // if (re.Businesstype == (short)businessType.changeContact)
                        // // {
                        // var userp = db.Aouser.FirstOrDefault(h => h.Identity == re.Identity);
                        // if (userp != null)
                        // {
                        //     phone = userp.Phone;
                        // }
                        // //  }
                        var line = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                         re.Identity, ((businessType)re.Businesstype).ToString(), re.Postaddr, re.Acceptingplace,
                         re.QuasiDrivingLicense, aouser.Phone, aouser.Name, re.Losttime,re.Abroadorservice,date);
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

        private static bool checkSignpic(businessType bt, string id, string home)
        {
            var picp = string.Format("{2}/server/pictures/{0}/{1}", id, bt.ToString(), home);
            if (!Directory.Exists(picp)) return false;
            var pp = new DirectoryInfo(picp).GetFiles();
            foreach (var ff in pp)
            {
                if (ff.Name.Contains(picType.declaration_sign.ToString())) return true;
            }
            return false;
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
