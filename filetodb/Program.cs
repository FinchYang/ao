using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        static string importPath = "ftp/get";

        static void Main(string[] args)
        {
            Console.WriteLine("import stated {0}!", DateTime.Now);
            FileToDb();
            Console.WriteLine("import completed {0}!", DateTime.Now);
        }
        static void FileToDb()
        {
            using (var db = new blahContext())
            {
                var filebase = "result.txt";
                 var home = Environment.GetEnvironmentVariable("HOME");
                var fname = Path.Combine(home,importPath, filebase);
                if (!File.Exists(fname))
                {
                    Console.WriteLine("file {0} does  not exist, exit.{1}", fname, DateTime.Now);
                    return;
                }
                var content = File.ReadAllLines(fname);
                foreach (var line in content)
                {
                    var fields = line.Split(',');
                    if (fields.Length < 5)
                    {
                        Console.WriteLine(" invalid data line {0},{1}", fields.Length, line);
                        continue;
                    }

                    var identity = fields[0];

                    var btype = fields[1];
                    var bbtype = businessType.unknown;
                    if (!Enum.TryParse(btype, out bbtype))
                    {
                        continue;
                    }
                    var success = fields[2];
                    var desc = fields[3];
                    var timed = fields[4];
                    var timedd=DateTime.Now;
                    DateTime.TryParse(timed, out timedd);
                    switch (success)
                    {
                        case "1":
                        case "2":
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
                            var busi = db.Business.FirstOrDefault(b => b.Identity == identity && b.Businesstype == (short)bbtype);
                            if (busi != null)
                            {
                                db.Businesshis.Add(new Businesshis
                                {
                                    Identity = busi.Identity,
                                    Businesstype = busi.Businesstype,
                                    Time = DateTime.Now,
                                });
                                db.Business.Remove(busi);
                            }
                            db.SaveChanges();
                            break;
                        case "3":
                            var busi3 = db.Business.FirstOrDefault(b => b.Identity == identity && b.Businesstype == (short)bbtype);
                            if (busi3 != null)
                            {
                                busi3.Status = (short)businessstatus.process;
                             //   busi3.Reason=desc;
                                busi3.Processtime=timedd;
                                db.SaveChanges();
                            }

                            break;
                        case "4":
                        var busi4 = db.Business.FirstOrDefault(b => b.Identity == identity && b.Businesstype == (short)bbtype);
                            if (busi4 != null)
                            {
                                busi4.Status = (short)businessstatus.failure;
                                busi4.Reason=desc;
                                busi4.Finishtime=timedd;
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
