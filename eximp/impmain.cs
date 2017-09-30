using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using eximp;

namespace importdata
{
    class Program
    {
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
        public enum businessstatus
        {
            unknown,
            wait,//身份证正面
            process,//身份证反面
            finish, //户口簿本人信息变更页  
            failure
        };
        static string importPath = "ftp/get/netban";

        static void Main(string[] args)
        {
            Console.WriteLine("import stated {0}!", DateTime.Now);
            FileToDb();
            Console.WriteLine("import completed {0}!", DateTime.Now);
        }
        static void clean(enaboContext db, string identity, businessType bbtype, DateTime dsubmit)
        {

            var busi = db.Business.FirstOrDefault(b => b.Identity == identity && b.Businesstype == (short)bbtype
             && b.Integrated == true
             && b.Exporttime == dsubmit);
            if (busi != null)
            {
                db.Businesshis.Add(new Businesshis
                {
                    Identity = busi.Identity,
                    Businesstype = busi.Businesstype,
                    Time = DateTime.Now,
                });
                db.Business.Remove(busi);
                var pics = db.Businesspic.Where(a => a.Identity == identity && a.Businesstype == (int)bbtype);
                foreach (var pic in pics)
                {
                    db.Businesspichis.Add(new Businesspichis
                    {
                        Identity = pic.Identity,
                        Time = DateTime.Now,
                        Businesstype = (short)pic.Businesstype,
                        Pictype = pic.Pictype
                    });
                    db.Businesspic.Remove(pic);
                }
            }
            db.SaveChanges();
        }
        static void FileToDb()
        {
            using (var db = new enaboContext())
            {
                var filebase = "result.txt";
                var home = Environment.GetEnvironmentVariable("HOME");
                var fname = Path.Combine(home, importPath, filebase);
                if (!File.Exists(fname))
                {
                    Console.WriteLine("file {0} does  not exist, exit.{1}", fname, DateTime.Now);
                    return;
                }
                var content = File.ReadAllLines(fname);
                Console.WriteLine("{0} lines need to import", content.Length);
                foreach (var line in content)
                {
                    var fields = line.Split(',');
                    if (fields.Length < 6)
                    {
                        Console.WriteLine(" invalid data line {0},{1}", fields.Length, line);
                        continue;
                    }

                    var identity = fields[0];
                    var btype = fields[1];
                    var bbtype = businessType.unknown;
                    if (!Enum.TryParse(btype, out bbtype))
                    {
                        Console.WriteLine(" businessType error line {0},{1}", fields.Length, line);
                        continue;
                    }
                    var success = fields[2];
                    var desc = fields[3];
                    var timed = fields[4];
                    var timedd = DateTime.Now;
                    if (!DateTime.TryParse(timed, out timedd))
                    {
                        Console.WriteLine(" process time error line {0},{1}", timed, line);
                        // Console.WriteLine(" businessType ={0},identity={1},success={2},desc={3},timedd={4},bbtype={5}",
                        //     btype, identity, success, desc, timedd, (int)bbtype);
                        continue;
                    }
                    var submittime = fields[5];
                    var dsubmit = DateTime.Now;
                    if (!DateTime.TryParse(submittime, out dsubmit))
                    {
                        Console.WriteLine(" submittime error line {0},{1}", submittime, line);
                        continue;
                    }
                    switch (success)
                    {
                        case "1":
                            clean(db, identity, bbtype, dsubmit);
                            break;
                        case "2":
                            clean(db, identity, bbtype, dsubmit);
                            var tuser = db.Aouser.FirstOrDefault(a => a.Identity == identity);
                            if (tuser != null)
                            {
                                tuser.Blacklist = true;
                                db.SaveChanges();
                            }
                            break;
                        case "3":
                            var busi3 = db.Business.FirstOrDefault(b => b.Identity == identity && b.Businesstype == (short)bbtype
                             && b.Integrated == true
                             && b.Exporttime == dsubmit);
                            if (busi3 != null)
                            {
                                busi3.Status = (short)businessstatus.process;
                                //   busi3.Reason=desc;
                                busi3.Processtime = timedd;
                                db.SaveChanges();
                            }

                            break;
                        case "4":
                            var busi4 = db.Business.FirstOrDefault(b => b.Identity == identity
                             && b.Businesstype == (short)bbtype
                             && b.Integrated == true && b.Exporttime == dsubmit);
                            if (busi4 != null)
                            {
                                busi4.Status = (short)businessstatus.failure;
                                busi4.Reason = desc;
                                busi4.Finishtime = timedd;
                                db.SaveChanges();
                            }
                            break;
                        case "7":
                            clean(db, identity, bbtype, dsubmit);
                            break;
                        case "8":
                            var tuser8 = db.Aouser.FirstOrDefault(a => a.Identity == identity);
                            if (tuser8 != null)
                            {
                                tuser8.Blacklist = false;
                                db.SaveChanges();
                            }
                            break;
                        default:
                            Console.WriteLine(" invalid data line {0},{1}", fields.Length, line);
                            break;
                    }
                }
            }
        }
    }
}
