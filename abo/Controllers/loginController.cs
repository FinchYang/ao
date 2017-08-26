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
    public class loginController : Controller
    {
        public readonly ILogger<loginController> _log;

        private readonly blahContext _db1 = new blahContext();
        static string _picpath = "pictures";
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                _db1.Dispose();
            }
            base.Dispose(disposing);
        }
        public loginController(ILogger<loginController> log)
        {
            _log = log;
        }

        private string GetToken()
        {
            var seed = Guid.NewGuid().ToString("N");
            return seed;
        }
      

        [Route("login")]
        [HttpGet]
        public loginresponse login(string name, string identify, string phone, businessType businessType)
        {
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
            identify + name + phone);

            var theuser = _db1.User.FirstOrDefault(i => i.Identity == identify);
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
