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
                var acc = new access_idinfo { Identity = string.Empty, businessType = businessType.basicinfo, status = responseStatus.ok };
                foreach (var a in global.tokens)
                {
                    if (a.Token == htoken)
                    {
                        acc.Identity = a.idinfo.Identity;
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
                    acc.businessType = ci.businessType;
                }
                LogRequest(acc.Identity, acc.businessType.ToString());
                return acc;
            }
            catch (Exception ex)
            {
                return new access_idinfo { status = responseStatus.tokenerror, content = ex.Message };
            }
        }
        public static async void LogRequest(string content, string method = null, string ip = null, short businessType = 0)
        {
            var dbtext = string.Empty;
            var dbmethod = string.Empty;
            var dbip = string.Empty;
            var contentlenth = 150;
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
                using (var logdb = new aboContext())
                {
                    logdb.Request.Add(new Request
                    {
                        Content = dbtext,
                        Businesstype = businessType,
                        Ip = dbip,
                        Method = dbmethod,
                        Time = DateTime.Now
                    });
                    logdb.SaveChanges();
                }
            });

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

        public static List<Ptoken> tokens = new List<Ptoken>();
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
}