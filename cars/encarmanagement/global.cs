using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using mvc104.models;
using Newtonsoft.Json;
using StackExchange.Redis;
using static mvc104.global;
using enabo;
using encm.cars;

namespace mvc104
{
    public class highlevel
    {
        public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.10.94:6379");
        //   public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("47.93.226.74:8111");
        public static access_idinfo GetInfoByToken(IHeaderDictionary header)
        {
            try
            {
                var htoken = header["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new access_idinfo { status = responseStatus.tokenerror };
                }
                var found = false;
                var acc = new access_idinfo { Identity = string.Empty, businessType = businessType.unknown, status = responseStatus.ok };
                foreach (var a in global.tokens)
                {
                    if (a.Token == htoken)
                    {
                        acc.Identity = a.idinfo.Identity;
                        acc.photofile = a.idinfo.photofile;
                        acc.businessType = a.idinfo.businessType;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    var redisdb = highlevel.redis.GetDatabase();
                    var cacheidinfo = redisdb.StringGet(htoken);
                    if (cacheidinfo == "nil")
                    {
                        return new access_idinfo { status = responseStatus.tokenerror };
                    }
                    var ci = JsonConvert.DeserializeObject<idinfo>(cacheidinfo);
                    acc.Identity = ci.Identity;
                    acc.photofile = ci.photofile;
                    acc.businessType = ci.businessType;
                }
            //    LogRequest(acc.Identity, acc.businessType.ToString());
                return acc;
            }
            catch (Exception ex)
            {
                return new access_idinfo { status = responseStatus.tokenerror, content = ex.Message };
            }
        }
        public static async Task LogRequest(string content, string method = null, string ip = null, short businessType = 0)
        {
            try{
            var dbtext = string.Empty;
            var dbmethod = string.Empty;
            var dbip = string.Empty;
            var contentlenth = 4150;
            var shortlength = 44;
          
            if (!string.IsNullOrEmpty(content))
            {
                var lenth = content.Length;
                dbtext = lenth > contentlenth ? content.Substring(0, contentlenth) : content;
            }
            if (!string.IsNullOrEmpty(method))
            {
                dbmethod = method.Length > shortlength ? method.Substring(0, shortlength) : method;
            }
            if (!string.IsNullOrEmpty(ip))
            {
                dbip = ip.Length > shortlength ? ip.Substring(0, shortlength) : ip;
            }
            await Task.Run(() =>
            {
                using (var logdb = new carsContext())
                {
                    logdb.Carslog.Add(new Carslog
                    {
                        Content = dbtext,
                        Businesstype = businessType,
                        Ip = dbip,
                        Method = dbmethod,
                        Time = DateTime.Now
                    });
                    logdb.SaveChanges();
                }
            });}
            catch(Exception ex){
                Console.WriteLine("db log error :{0}",ex.Message);
            }
        }
     
        public static void errorlog(ILogger log, string method, Exception ex)
        {
            log.LogError("{0}-{1}-{2}", DateTime.Now, method, ex.Message);
        }
        public static commonresponse commonreturn(responseStatus rs)
        {
            return new commonresponse { status = rs, content = rs.ToString() };
        }
        public static void infolog(ILogger log, string method, string msg)
        {
            log.LogInformation("{0}-{1}-{2}", DateTime.Now, method, msg);
        }
    }

    public static partial class global
    {
        public static Dictionary<businessType, short> businesscount = new Dictionary<businessType, short>();
        public static Dictionary<businessType, string> statistics = new Dictionary<businessType, string>();
        public static List<Ptoken> tokens = new List<Ptoken>();
        static global()
        {
            statistics.Add(businessType.unknown, "合计");

            //statistics.Add(businessType.ChangeLicense, "变更户籍姓名");//变更户籍姓名
            //statistics.Add(businessType.delay, "延期换证");//延期换证?
            //statistics.Add(businessType.lost, "遗失补证");//遗失补证
            //statistics.Add(businessType.damage, "损毁换证");//损毁换证
            //statistics.Add(businessType.overage, "超龄换证");//超龄换证?

            //statistics.Add(businessType.expire, "期满换证");//期满换证??6,7??
            //statistics.Add(businessType.changeaddr, "变更户籍地址");//变更户籍地址
            //statistics.Add(businessType.basicinfo, "基本信息证明");//基本信息证明
            //statistics.Add(businessType.first, "驾驶证自愿业务退办");//初领、增加机动车驾驶证自愿业务退办
            //statistics.Add(businessType.network, "网约车安全驾驶证明");// 网约车安全驾驶证明

            //statistics.Add(businessType.three, "三年无重大事故证明");//三年无重大事故证明
            //statistics.Add(businessType.five, "五年安全驾驶证明");//五年安全驾驶证明

            //statistics.Add(businessType.inspectDelay, "延期审验");//延期审验
            //statistics.Add(businessType.bodyDelay, "延期提交身体证明");//延期提交身体证明
            //statistics.Add(businessType.changeContact, "变更联系方式");//变更联系方式

            businesscount.Add(businessType.unknown, 5);

            businesscount.Add(businessType.newplate, 5);//补领机动车号牌
            businesscount.Add(businessType.scrap, 0);//注销登记-车辆报废
            businesscount.Add(businessType.changeplate, 6);//换领机动车号牌
            businesscount.Add(businessType.newlicense, 6);//补领机动车行驶证
            businesscount.Add(businessType.changelicense, 7);//换领机动车行驶证

            businesscount.Add(businessType.newtag, 8);//补领检验合格标志
            businesscount.Add(businessType.changetag, 9);//换领检验合格标志

            //businesscount.Add(businessType.basicinfo, 4);//基本信息证明
            //businesscount.Add(businessType.first, 4);//初领、增加机动车驾驶证自愿业务退办
            //businesscount.Add(businessType.network, 4);// 网约车安全驾驶证明

            //businesscount.Add(businessType.three, 4);//三年无重大事故证明
            //businesscount.Add(businessType.five, 4);//五年安全驾驶证明

            //businesscount.Add(businessType.inspectDelay, 6);//延期审验
            //businesscount.Add(businessType.bodyDelay, 6);//延期提交身体证明
            businesscount.Add(businessType.changecontact, 4);//变更联系方式
        }
    }
}