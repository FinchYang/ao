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

        //  static  string baseurl="https://192.168.10.27:443/data/";
        //      static  string slog="20170629172";
        //   static string skey="ZDL02001201706";
        //   static string publickey="MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDTleXSqOajvEwt8RhDYNZd4guh"+
        // "DiGWgeFiSt4c1eFigalhaGB6+KL6PSV3wEMgbe2x1fKJoS52Qi7Vxu4w64TS5xmB"+
        // "DUzcndO9FhjGYO1CoHIyO9AtczzPBePDYcd2tk+gjMSpf+Z3jMnTGVgRDBkSpqi9"+
        // "YbnUmtfA8JpVJEviMwIDAQAB";
        // static string clientPrivateKey="MIICXAIBAAKBgQDTleXSqOajvEwt8RhDYNZd4guhDiGWgeFiSt4c1eFigalhaGB6"+
        // "+KL6PSV3wEMgbe2x1fKJoS52Qi7Vxu4w64TS5xmBDUzcndO9FhjGYO1CoHIyO9At"+
        // "czzPBePDYcd2tk+gjMSpf+Z3jMnTGVgRDBkSpqi9YbnUmtfA8JpVJEviMwIDAQAB"+
        // "AoGAVwib5tGPPd7gvy0jO+QDic7H1dIIQu7eFR6Syu23rluDnwveU/ceoyyv0tiF"+
        // "RDuzwKkvASoKAJ8swMb5h6n5kkABOQzMNz2aozisssFA7QoA5QChdLFyaHsKmfTo"+
        // "mLOXVvsQKutQFWA2a/2wjMgtuy7cM/TU2WL1pbh4pUDHYeECQQDpKwIqTiyBXJKE"+
        // "/QF+hmjdNMB0qsFYensxhiLYql+Y9bFO7tXdWTBdI4ZK3SW5W8RYFUCXI8jXa4PS"+
        // "SQynTWU7AkEA6E3dsnT97No5Lbiwym+t60u8et4wFGLaUhrXB54Sda9gW2ooiVH+"+
        // "9G7SK79Jq8pcG984ydGauMb1oSdGO85HaQJAVtw8vEHO9onj00LlMZskqXMjVtLd"+
        // "n/ZQukw74vblEfhFCyCR7xlwmOHI/06O5RQ4eo/ANg2Qnh9hRg8Mda6xTQJAcC4q"+
        // "ASO9+8LmGc42kYuc0SOhwTPKxA14oG2VqXgMMgie34ZETQvrst5RYA7f5LW0BUGm"+
        // "SR/UdCipl0fY1mR5ecqYe+BdNRg2I/pHx3L/y7aIYek=";
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
                submitted = false,
                content = "unknown",
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
                        response.submitted = true;
                        response.businessstatus = (businessstatus)business.Status;
                        if (!string.IsNullOrEmpty(business.Reason)) response.content = business.Reason;
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

        [Route("newlogin")]
        [HttpGet]
        public loginresponse newlogin(string name, string identify, string phone, businessType businessType)
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

            if (!checkbusi(identify, businessType))
            {
                return new loginresponse { status = responseStatus.forbidden };
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
                submitted = false,
                content = "unknown",
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
                        response.submitted = true;
                        response.businessstatus = (businessstatus)business.Status;
                        if (!string.IsNullOrEmpty(business.Reason)) response.content = business.Reason;
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

        private bool checkbusi(string identify, businessType businessType)
        {
            if (businessType == businessType.changeContact || businessType == businessType.first) return true;
            sendautomsgone(identify);
            var rpath = "/home/driverbusiness/detachment";
            var rfile = Path.Combine(rpath, identify);
            var source = System.IO.File.ReadAllText(rfile);

            string pattern = @"{.*}";
            var result = string.Empty;
            Match theMatch = Regex.Match(source, pattern);
            if (theMatch.Success)
            {
                int endindex = theMatch.Length;
                result = source.Substring(theMatch.Index, endindex);
                var ret=JsonConvert.DeserializeObject<getDrivingLicenseBySfzmhm>(result);
                if(ret.code=="200"&&ret.status=="1"&&ret.result[0].ZT=="A") return true;
            }
            else
            {
                //支队同步库出错情况下，暂时丢弃业务逻辑限制
                return true;
            }

            return false;
        }

        private static void sendautomsgone(string id)
        {
            var a = new System.Diagnostics.Process();
            a.StartInfo.FileName = "/home/driverbusiness/bin/searchdetach";
            a.StartInfo.UseShellExecute = true;
            a.StartInfo.Arguments = id;
            a.Start();
            a.WaitForExit();
        }
    }
}
