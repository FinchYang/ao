using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc104.models;
using Newtonsoft.Json;
using static mvc104.global;

namespace mvc104.Controllers
{
    public class examController : Controller
    {
        public readonly ILogger<examController> _log;

        private readonly aboContext _db1 = new aboContext();      
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db1.Dispose();
            }
            base.Dispose(disposing);
        }
        public examController(ILogger<examController> log)
        {
            _log = log;
        }

        private string GetToken()
        {
            var seed = Guid.NewGuid().ToString("N");
            return seed;
        }      

  private string exam()
        {
           
            var url = string.Format("http://jisujiakao.market.alicloudapi.com/driverexam/query?pagenum=1&pagesize=1&sort=rand&subject=1&type=A1", 
            "wx774a9869c14f1647", "7f94f888c5e5c32bba9239230f46a827");
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

                using (var restget = new HttpClient(handler))
                {
                    var auth=new List<string>();
                    auth.Add("APPCODE a7686da69b354d78b1cd97b49ebd4490");
                    restget.DefaultRequestHeaders.Add("Authorization",auth);
                    var response = restget.GetAsync(url).Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                   
                    return srcString;
                }
            }
            catch (Exception ex)
            {
                highlevel.errorlog(_log, "exam", ex);
                return  "000001" ;
            }
        }
        [Route("lo77gin")]
        [HttpGet]
        public loginresponse login(string name, string identify, string phone, businessType businessType)
        {
           var ret= exam();
            return new loginresponse { status = responseStatus.ok,token=ret };
            if (string.IsNullOrEmpty(identify))
            {
                return new loginresponse { status = responseStatus.iderror };
            }
            if (string.IsNullOrEmpty(name))
            {
                return new loginresponse { status = responseStatus.nameerror };
            }
            if (string.IsNullOrEmpty(phone))
            {
                return new loginresponse { status = responseStatus.phoneerror };
            }
            _log.LogInformation("{3}-{0} from {1}, input is {2}", DateTime.Now, "login",
             Request.HttpContext.Connection.RemoteIpAddress.ToString() + HttpContext.Connection.RemoteIpAddress,
            identify + name + phone+businessType);

            var theuser = _db1.Aouser.FirstOrDefault(i => i.Identity == identify);
            if (theuser == null)
            {
                return new loginresponse { status = responseStatus.iderror };
            }

            theuser.Name = name;
            theuser.Phone = phone;
            _db1.SaveChanges();

            var btype = (short)businessType;
            var picsl = new List<short>();
            // var unbusiness = _db1.Business.FirstOrDefault(c => c.Completed == false && c.Businesstype == btype);
            // if(_db1.Businesspic.Count(c=>c.Businesstype==(short)businessType&&c.Identity==identify&&c.Uploaded)<global.businesscount[businessType])
            //  if (unbusiness != null)
            // {
            var pics = _db1.Businesspic.Where(c => c.Businesstype == btype && c.Identity == identify && c.Uploaded == true);
            if (pics.Count() < global.businesscount[businessType])
                foreach (var a in pics)
                {
                    picsl.Add(a.Pictype);
                }
            //  }
            var token = GetToken();
            try
            {
                var redisdb = highlevel.redis.GetDatabase();
                redisdb.StringSet(token, JsonConvert.SerializeObject(new idinfo { Identity = identify, businessType = businessType }));
                redisdb.KeyExpire(token, TimeSpan.FromDays(30));
            }
            catch (Exception ex)
            {
                _log.LogError("redis key process error:{0}", ex.Message);
            }

            var found = false;
            foreach (var a in global. tokens)
            {
                if (a.idinfo.Identity == identify && a.idinfo.businessType == businessType)
                {
                    a.Token = token;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                global. tokens.Add(new Ptoken { idinfo = new idinfo { Identity = identify, businessType = businessType }, Token = token });
            }
           highlevel. LogRequest(name + phone + identify, "login", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)businessType);
            return new loginresponse { status = responseStatus.ok, token = token, okpic = picsl.ToArray() };
        }
    }
}
