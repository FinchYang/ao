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
            if (string.IsNullOrEmpty(identify) || identify == "undefined")
            {
                return new loginresponse { status = responseStatus.iderror };
            }
            if (string.IsNullOrEmpty(name) || name == "undefined")
            {
                return new loginresponse { status = responseStatus.nameerror };
            }
            if (string.IsNullOrEmpty(phone) || phone == "undefined")
            {
                return new loginresponse { status = responseStatus.phoneerror };
            }
            if (businessType == businessType.unknown)
            {
                return new loginresponse { status = responseStatus.businesstypeerror };
            }

            _log.LogInformation("{3}-{0} from {1}, input is {2}", DateTime.Now, "login",
             Request.HttpContext.Connection.RemoteIpAddress.ToString() + HttpContext.Connection.RemoteIpAddress,
            identify + name + phone + businessType);

            var btype = (short)businessType;
            var picsl = new List<short>();
            var token = GetToken();
            var response = new loginresponse
            {
                status = responseStatus.ok,
                businessstatus = businessstatus.unknown,
                submitted=false,
                token = token,
                okpic = picsl.ToArray()
            };
            try
            {
                var theuser = _db1.Aouser.FirstOrDefault(i => i.Identity == identify);
                if (theuser == null)
                {
                    _db1.Aouser.Add(new Aouser
                    {
                        Identity = identify,
                        Phone = phone,
                        Name = name
                    });
                    _db1.SaveChanges();
                }

                var business = _db1.Business.FirstOrDefault(i => i.Identity == identify && i.Businesstype == (short)businessType);
                if (business == null)
                {
                    _db1.Business.Add(new Business
                    {
                        Identity = identify,
                        Businesstype = (short)businessType,
                        Completed = false,
                        Time = DateTime.Now,
                    });
                    _db1.SaveChanges();
                }
                else
                {
                    var pics = _db1.Businesspic.Where(c => c.Businesstype == btype && c.Identity == identify && c.Uploaded == true);
                     response.businessstatus = (businessstatus)business.Status;
                    if (pics.Count() < global.businesscount[businessType])
                    {
                        foreach (var a in pics)
                        {
                            picsl.Add(a.Pictype);
                        }
                        response.okpic = picsl.ToArray();
                    }
                    else
                    {
                        response.submitted=true;
                        response.businessstatus = (businessstatus)business.Status;
                        response.finish_time = business.Finishtime;
                        response.wait_time = business.Waittime;
                        response.process_time = business.Processtime;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError("login -- process error:{0}", ex.Message);
            }

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
            foreach (var a in global.tokens)
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
                global.tokens.Add(new Ptoken { idinfo = new idinfo { Identity = identify, businessType = businessType }, Token = token });
            }
            highlevel.LogRequest(name + phone + identify, "login", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)businessType);
            return response;
        }
    }
}
