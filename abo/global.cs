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
        public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("47.93.226.74:8111");
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
                using (var logdb = new blahContext())
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
            businesscount.Add(businessType.ChangeLicense, 5);
            businesscount.Add(businessType.delay, 4);
            businesscount.Add(businessType.lost, 3);
            businesscount.Add(businessType.damage, 5);
            businesscount.Add(businessType.overage, 5);

            businesscount.Add(businessType.expire, 5);
            businesscount.Add(businessType.changeaddr, 5);
            businesscount.Add(businessType.basicinfo, 4);
            businesscount.Add(businessType.first, 3);
            businesscount.Add(businessType.network, 4);

            businesscount.Add(businessType.three, 4);
            businesscount.Add(businessType.five, 4);
        }
    }
}