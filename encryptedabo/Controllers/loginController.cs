using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        private readonly aboContext _db1 = new aboContext();

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

                  var btype = (short)businessType;
            var picsl = new List<short>();
            var token = GetToken();
            var photofile = string.Empty;
            var response = new loginresponse
            {
                status = responseStatus.ok,blacklist=false,
                businessstatus = businessstatus.unknown,
                submitted = false,
                content = "unknown",
                token = token,
                okpic = picsl.ToArray()
            };
              var encryptedIdentity=CryptographyHelpers.StudyEncrypt(identify);
            try
            {
                if (businessType == businessType.overage)
                {
                    var idl = identify.Length;
                    if (idl == 18)//
                    {
                        var year = int.Parse(identify.Substring(6, 4));
                        var month = int.Parse(identify.Substring(10, 2));
                        var day = int.Parse(identify.Substring(12, 2));
                        var birth = new DateTime(year, month, day);
                        if (birth.AddYears(60) > DateTime.Now) return new loginresponse { status = responseStatus.forbidden };
                    }
                    else if (idl == 15)
                    {
                        var year = int.Parse(identify.Substring(6, 2)) + 1900;
                        var month = int.Parse(identify.Substring(8, 2));
                        var day = int.Parse(identify.Substring(10, 2));
                        var birth = new DateTime(year, month, day);
                        if (birth.AddYears(60) > DateTime.Now) return new loginresponse { status = responseStatus.forbidden };
                    }
                }
              
                var theuser = _db1.Aouser.FirstOrDefault(i => i.Identity==encryptedIdentity);
                if (theuser == null)
                {
                    photofile = token;
                    _db1.Aouser.Add(new Aouser
                    {
                        Identity = encryptedIdentity,
                        Blacklist=false,
                        Photofile=token,
                        Phone = phone,
                        Name = name
                    });
                    _db1.SaveChanges();
                }
                else
                {
                    photofile = theuser.Photofile;
                }

                var business = _db1.Business.FirstOrDefault(i =>i.Identity==encryptedIdentity
                 && i.Businesstype == (short)businessType);
                if (business == null)
                {
                    _db1.Business.Add(new Business
                    {
                        Identity = encryptedIdentity,
                        Businesstype = (short)businessType,
                        Completed = false,
                        Time = DateTime.Now,
                    });
                    _db1.SaveChanges();
                }
                else
                {
                    var pics = _db1.Businesspic.Where(c => c.Businesstype == btype
                     && c.Identity==encryptedIdentity
                      && c.Uploaded == true);
                    response.businessstatus = (businessstatus)business.Status;
                      response.finish_time = business.Finishtime;
                        response.wait_time = business.Waittime;
                        response.process_time = business.Processtime;
                          if (!string.IsNullOrEmpty(business.Reason)) response.content = business.Reason;
                    if (pics.Count() < global.businesscount[businessType] || response.businessstatus == businessstatus.failure)
                    {
                        foreach (var a in pics)
                        {
                            picsl.Add(a.Pictype);
                        }
                        response.okpic = picsl.ToArray();
                        if(response.businessstatus == businessstatus.failure)  response.submitted = true;
                    }
                    else
                    {
                        response.submitted = true;
                    }
                }
                if (theuser != null&&theuser.Blacklist==true)
                {
                   response.blacklist =true;
                }
            }
            catch (Exception ex)
            {
                _log.LogError("login -- process error:{0}", ex.Message);
            }

            try
            {
                var redisdb = highlevel.redis.GetDatabase();
                redisdb.StringSet(token, JsonConvert.SerializeObject(new idinfo { Identity = encryptedIdentity,photofile=photofile, businessType = businessType }));
                redisdb.KeyExpire(token, TimeSpan.FromDays(30));
            }
            catch (Exception ex)
            {
                _log.LogError("redis key process error:{0}", ex.Message);
            }

            var found = false;
            foreach (var a in global.tokens)
            {
                if (a.idinfo.Identity == encryptedIdentity && a.idinfo.businessType == businessType)
                {
                    a.Token = token;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                global.tokens.Add(new Ptoken { idinfo = new idinfo { Identity = encryptedIdentity, photofile = photofile, businessType = businessType }, Token = token });
            }
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he + name + phone + encryptedIdentity+ JsonConvert.SerializeObject(response),
                 "login", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)businessType));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return response;
        }
    }
}
