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

namespace mvc104.Controllers
{
    public class loginController : Controller
    {
        public readonly ILogger<loginController> _log;

        private readonly blahContext _db1 = new blahContext();
        static List<Ptoken> tokens = new List<Ptoken>();
        static tokenticket _tt = new tokenticket();
        static string _picpath = "pictures";
        class Ptoken
        {
            public idinfo idinfo { get; set; }
            public string Token { get; set; }
        }
        class idinfo
        {
            public string Identity { get; set; }
            public businessType businessType { get; set; }
        }


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
        private async void LogRequest(string content, string method = null, string ip = null, short businessType = 0)
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
        [Route("declarationSign")]
        [HttpPost]
        public commonresponse declarationSign([FromBody]declarationsignrequest input)
        {
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            var identity = string.Empty;
            var btype = businessType.basicinfo;
            try
            {
                var htoken = Request.Headers["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new commonresponse { status = responseStatus.tokenerror };
                }
                var found = false;

                foreach (var a in tokens)
                {
                    if (a.Token == htoken)
                    {
                        identity = a.idinfo.Identity;
                        btype = a.idinfo.businessType;
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
                        return new commonresponse { status = responseStatus.tokenerror };
                    }
                    var ci = JsonConvert.DeserializeObject<idinfo>(cacheidinfo);
                    identity = ci.Identity;
                    btype = ci.businessType;
                }
            }
            catch (Exception ex)
            {
                return new commonresponse { status = responseStatus.tokenerror };
            }

            // if (string.IsNullOrEmpty(input.id_back))
            // {
            //     return new commonresponse { status = responseStatus.imageerror };
            // }

            if (!savePic(input.sign_pic, picType.declaration_sign, identity, btype))
                return new commonresponse { status = responseStatus.fileprocesserror };
            return new commonresponse { status = responseStatus.ok };
        }
        //   public tokenProcess(HttpRequest header)
        [Route("downloadpic")]
        [HttpGet]
        public commonresponse downloadpic(picType picType)
        {
            LogRequest("downloadpic", "downloadpic", Request.HttpContext.Connection.RemoteIpAddress.ToString());

            var identity = string.Empty;
            var btype = businessType.basicinfo;
            try
            {
                var htoken = Request.Headers["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new commonresponse { status = responseStatus.tokenerror };
                }
                var found = false;

                foreach (var a in tokens)
                {
                    if (a.Token == htoken)
                    {
                        identity = a.idinfo.Identity;
                        btype = a.idinfo.businessType;
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
                        return new commonresponse { status = responseStatus.tokenerror };
                    }
                    var ci = JsonConvert.DeserializeObject<idinfo>(cacheidinfo);
                    identity = ci.Identity;
                    btype = ci.businessType;
                }
            }
            catch (Exception ex)
            {
                return new commonresponse { status = responseStatus.tokenerror };
            }

            try
            {
                var fname = Path.Combine(_picpath, identity, btype.ToString(), picType + ".jpg");
                var bbytes = System.IO.File.ReadAllBytes(fname);
                var retstr = Convert.ToBase64String(bbytes);
                return new downloadresponse { status = responseStatus.ok, picture = retstr };
            }
            catch (Exception ex)
            {
                return new commonresponse { status = responseStatus.processerror };
            }

        }
        [Route("uploadpic")]
        [HttpPost]
        public commonresponse uploadpic([FromBody]uploadpicrequest input)
        {
            LogRequest("uploadpic", "uploadpic", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            var identity = string.Empty;
            var btype = businessType.basicinfo;
            try
            {
                var htoken = Request.Headers["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new commonresponse { status = responseStatus.tokenerror };
                }
                var found = false;

                foreach (var a in tokens)
                {
                    if (a.Token == htoken)
                    {
                        identity = a.idinfo.Identity;
                        btype = a.idinfo.businessType;
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
                        return new commonresponse { status = responseStatus.tokenerror };
                    }
                    var ci = JsonConvert.DeserializeObject<idinfo>(cacheidinfo);
                    identity = ci.Identity;
                    btype = ci.businessType;
                }
            }
            catch (Exception ex)
            {
                return new commonresponse { status = responseStatus.tokenerror };
            }

            // if (string.IsNullOrEmpty(input.id_back))
            // {
            //     return new commonresponse { status = responseStatus.imageerror };
            // }

            if (!savePic(input.picture, input.picType, identity, btype))
                return new commonresponse { status = responseStatus.fileprocesserror };

            _db1.Businesspic.Add(new Businesspic
            {
                Identity = identity,
                Businesstype = (short)btype,
                Pictype = (short)input.picType,
                Uploaded = true
            });
            _db1.SaveChanges();

            return new commonresponse { status = responseStatus.ok };
        }
        [Route("updateinfo")]
        [HttpPost]
        public commonresponse updateinfo([FromBody]updateinforequest input)
        {
            LogRequest("updateinfo", "updateinfo", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            var identity = string.Empty;
            try
            {
                var htoken = Request.Headers["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new commonresponse { status = responseStatus.tokenerror };
                }
                var found = false;

                foreach (var a in tokens)
                {
                    if (a.Token == htoken)
                    {
                        identity = a.idinfo.Identity;
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
                        return new commonresponse { status = responseStatus.tokenerror };
                    }
                    return new commonresponse { status = responseStatus.tokenerror };
                }
            }
            catch (Exception ex)
            {
                return new commonresponse { status = responseStatus.tokenerror };
            }

            if (string.IsNullOrEmpty(input.postaddr))
            {
                return new commonresponse { status = responseStatus.postaddrerror };
            }

            try
            {
                var theuser = _db1.User.FirstOrDefault(i => i.Identity == identity);
                if (theuser == null)
                {
                    return new commonresponse { status = responseStatus.iderror };
                }
                theuser.Postaddr = input.postaddr;
                _db1.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return new commonresponse { status = responseStatus.dberror };
            }
            return new commonresponse { status = responseStatus.ok };
        }
        [Route("ChangeLicense")]
        [HttpPost]
        public commonresponse ChangeLicense([FromBody]changelicenserequest input)
        {
            LogRequest("ChangeLicense", "ChangeLicense", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            var btype = businessType.basicinfo;
            var identity = string.Empty;
            try
            {
                var htoken = Request.Headers["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new commonresponse { status = responseStatus.tokenerror };
                }
                var found = false;

                foreach (var a in tokens)
                {
                    if (a.Token == htoken)
                    {
                        identity = a.idinfo.Identity;
                        btype = a.idinfo.businessType;
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
                        return new commonresponse { status = responseStatus.tokenerror };
                    }
                    var ci = JsonConvert.DeserializeObject<idinfo>(cacheidinfo);
                    identity = ci.Identity;
                    btype = ci.businessType;
                }
            }
            catch (Exception ex)
            {
                return new commonresponse { status = responseStatus.tokenerror };
            }

            // if (string.IsNullOrEmpty(input.id_back))
            // {
            //     return new commonresponse { status = responseStatus.imageerror };
            // }
            if (input.lost)
            {
                if (!savePic(input.sign_pic, picType.sign_pic, identity, btype))
                    return new commonresponse { status = responseStatus.fileprocesserror };
            }
            else
            {
                if (!savePic(input.license_pic, picType.driver, identity, btype))
                    return new commonresponse { status = responseStatus.fileprocesserror };
            }
            if (!savePic(input.id_back, picType.id_back, identity, btype))
                return new commonresponse { status = responseStatus.fileprocesserror };
            if (!savePic(input.id_front, picType.id_front, identity, btype))
                return new commonresponse { status = responseStatus.fileprocesserror };
            if (!savePic(input.id_inhand, picType.id_inhand, identity, btype))
                return new commonresponse { status = responseStatus.fileprocesserror };
            if (!savePic(input.hukou_pic, picType.hukou_pic, identity, btype))
                return new commonresponse { status = responseStatus.fileprocesserror };
            return new commonresponse { status = responseStatus.ok };
        }
        private bool savePic(string picstr, picType picType, string identity, businessType btype)
        {
            try
            {
                var fpath = Path.Combine(_picpath, identity);
                if (!Directory.Exists(fpath)) Directory.CreateDirectory(fpath);
                var fname = Path.Combine(fpath, identity, btype.ToString(), picType + ".jpg");
                var index = picstr.IndexOf("base64,");
                System.IO.File.WriteAllBytes(fname, Convert.FromBase64String(picstr.Substring(index + 7)));
            }
            catch (Exception ex)
            {
                _log.LogInformation("savePic error: {0}", ex);
                return false;
            }
            return true;
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
            foreach (var a in tokens)
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
                tokens.Add(new Ptoken { idinfo = new idinfo { Identity = identify, businessType = businessType }, Token = token });
            }
            LogRequest(name + phone + identify, "login", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)businessType);
            return new loginresponse { status = responseStatus.ok, token = token, okpic = picsl.ToArray() };
        }

        [Route("FaceCompare")]
        [HttpPost]
        public commonresponse FaceCompare([FromBody]facerequest input)
        {
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            try
            {
                var inin = JsonConvert.SerializeObject(input);
                _log.LogInformation("input ={1},{0}", inin.Length);
                var htoken = Request.Headers["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new commonresponse { status = responseStatus.tokenerror };
                }
                var found = false;
                var identity = string.Empty;
                foreach (var a in tokens)
                {
                    if (a.Token == htoken)
                    {
                        identity = a.idinfo.Identity;
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
                        return new commonresponse { status = responseStatus.tokenerror };
                    }
                    return new commonresponse { status = responseStatus.tokenerror };
                }
            }
            catch (Exception ex)
            {
                return new wxconfigresponse { status = responseStatus.tokenerror };
            }

            if (string.IsNullOrEmpty(input.image))
            {
                return new commonresponse { status = responseStatus.imageerror };
            }
            var fname = Path.GetTempFileName() + ".jpg";
            try
            {

                var index = input.image.IndexOf("base64,");
                _log.LogInformation("length: {0}", input.image.Length);
                System.IO.File.WriteAllBytes(fname, Convert.FromBase64String(input.image.Substring(index + 7)));
            }
            catch (Exception ex)
            {
                _log.LogInformation("error: {0}", ex);
                return new commonresponse { status = responseStatus.fileprocesserror };
            }

            try
            {
                //  var fname = @"d:\ycl.jpg";
                // var req=new livingbodyrequest(){
                var api_id = "9a4c8ff73d6642d886c537403a0a736d";
                var api_secret = "d5f2e07d025b4bc8bdc8e4774f904fbf";
                //     file=System.IO.File.ReadAllBytes(fname)
                // };

                //     var bbytes=System.IO.File.ReadAllBytes(fname);
                //     var str64=Convert.ToBase64String(bbytes);
                //      var req=new livingbodyrequest2(){
                //         api_id="9a4c8ff73d6642d886c537403a0a736d",
                //         api_secret="d5f2e07d025b4bc8bdc8e4774f904fbf",
                //         file=str64
                //     };
                //    var theUrl="https://cloudapi.linkface.cn/hackness/selfie_hack_detect";
                //    var ret= SendRestHttpClientRequest(theUrl,JsonConvert.SerializeObject(req));
                var ret = living(api_id, api_secret, fname);
                _log.LogInformation("ret={0}", ret);
                var retsta = JsonConvert.DeserializeObject<okcheck>(ret);
                if (retsta.status != "OK")
                {
                    return new commonresponse { status = responseStatus.livingerror, content = ret };
                }
                var retok = JsonConvert.DeserializeObject<okcheck2>(ret);
                var score = double.Parse(retok.score);
                if (score >= 0.98)
                {
                    return new commonresponse { status = responseStatus.livingerror, content = ret };
                }
                var history = "test.jpg";
                var rettwo = living22(api_id, api_secret, fname, history);
                var twoc = JsonConvert.DeserializeObject<okcheck>(rettwo);
                if (twoc.status != "OK")
                {
                    return new commonresponse { status = responseStatus.compareerror, content = rettwo };
                }
                var twook = JsonConvert.DeserializeObject<okchecktwo>(rettwo);
                var confidence = double.Parse(twook.confidence);
                if (confidence <= 0.78)
                {
                    return new commonresponse { status = responseStatus.compareerror, content = rettwo };
                }
                return new commonresponse { status = responseStatus.ok, content = rettwo };
            }
            catch (Exception ex)
            {
                _log.LogInformation("error: {0}", ex);
                return new commonresponse { status = responseStatus.fileprocesserror };
            }
            return new commonresponse { status = responseStatus.ok };
        }
        private string living22(string api_id, string api_secret, string path, string historypath)
        {
            HttpContent apiId = new StringContent(api_id);
            HttpContent apiSecret = new StringContent(api_secret);
            HttpContent photo = new ByteArrayContent(System.IO.File.ReadAllBytes(path));
            HttpContent historyphoto = new ByteArrayContent(System.IO.File.ReadAllBytes(historypath));
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(apiId, "api_id");
                formData.Add(apiSecret, "api_secret");
                formData.Add(photo, "selfie_file", path);
                formData.Add(historyphoto, "historical_selfie_file", historypath);
                var response = client.PostAsync(
                    "https://cloudapi.linkface.cn/identity/historical_selfie_verification",
                    formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.Write(response.ToString());
                    return string.Empty;
                }
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        private string living(string api_id, string api_secret, string path)
        {
            HttpContent apiId = new StringContent(api_id);
            HttpContent apiSecret = new StringContent(api_secret);
            HttpContent photo = new ByteArrayContent(System.IO.File.ReadAllBytes(path));
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(apiId, "api_id");
                formData.Add(apiSecret, "api_secret");
                formData.Add(photo, "file", path);
                var response = client.PostAsync(
                    "https://cloudapi.linkface.cn/hackness/selfie_hack_detect",
                    formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.Write(response.ToString());
                    return string.Empty;
                }
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        private static string CompareWidthIdetify(string api_id, string api_secret, string identify, string name, string newpicpath, string idpicpath)
        {
            HttpContent apiId = new StringContent(api_id);
            HttpContent apiSecret = new StringContent(api_secret);
            HttpContent n = new StringContent(name);
            HttpContent id = new StringContent(identify);
            HttpContent photo = new ByteArrayContent(System.IO.File.ReadAllBytes(newpicpath));
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(apiId, "api_id");
                formData.Add(apiSecret, "api_secret");
                formData.Add(n, "name");
                formData.Add(id, "id_number");
                formData.Add(photo, "selfie_file", "test.jpg");
                var response = client.PostAsync(
                    "https://cloudapi.linkface.cn/identity/selfie_idnumber_verification",
                    formData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.Write(response.ToString());
                    return "";
                }
                return response.Content.ReadAsStringAsync().Result;
            }
        }
        [Route("getwxconfig")]
        [HttpGet]
        public wxconfigresponse getwxconfig(string url)
        {
            try
            {
                var htoken = Request.Headers["token"].First();
                if (string.IsNullOrEmpty(htoken))
                {
                    return new wxconfigresponse { status = responseStatus.tokenerror };
                }
                var found = false;
                var identity = string.Empty;

                foreach (var a in tokens)
                {
                    if (a.Token == htoken)
                    {
                        identity = a.idinfo.Identity;
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
                        return new wxconfigresponse { status = responseStatus.tokenerror };
                    }
                    var cacheid = JsonConvert.DeserializeObject<idinfo>(cacheidinfo);
                    identity = cacheid.Identity;
                }
            }
            catch (Exception ex)
            {
                _log.LogError("getwxconfig error:{0}", ex.Message);
                return new wxconfigresponse { status = responseStatus.tokenerror };
            }

            var ret = getAccessToken();
            _log.LogInformation("ret={0}", ret.access_token);
            if (ret.access_token == "000001")
            {
                return new wxconfigresponse { status = responseStatus.access_tokenerror };
            }
            if (string.IsNullOrEmpty(_tt.access_token) || _tt.access_token != ret.access_token)
            {
                _tt.access_token = ret.access_token;
                _tt.last = DateTime.Now;
                _tt.expires_in = ret.expires_in;
            }

            var ticket = getTicket(ret.access_token);
            _log.LogInformation("ticket={0}", ticket.ticket);
            if (ticket.ticket == "000001")
            {
                return new wxconfigresponse { status = responseStatus.ticketerror };
            }
            if (string.IsNullOrEmpty(_tt.ticket) || _tt.ticket != ticket.ticket)
            {
                _tt.ticket = ticket.ticket;
                _tt.ticketexpires_in = ticket.expires_in;
                _tt.ticketlast = DateTime.Now;
            }
            var nonce = "112";

            DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);

            var stamp = (DateTime.Now.Ticks - dt_1970.Ticks) / 10000000;
            var sn = getSignature(ticket.ticket, nonce, url, stamp);

            return new wxconfigresponse { status = responseStatus.ok, nonceStr = nonce, signature = sn, timestamp = stamp };
        }

        private accesstoken getAccessToken()
        {
            if (!string.IsNullOrEmpty(_tt.access_token))
            {
                if (DateTime.Now.CompareTo(_tt.last.AddSeconds(_tt.expires_in - 100)) < 0)
                {
                    return new accesstoken { access_token = _tt.access_token };
                }
            }
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", "wx7fc046cc9adb13e4", "b299bac11729dd892f903115be3aabd9");
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

                using (var restget = new HttpClient(handler))
                {
                    var response = restget.GetAsync(url).Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    var at = JsonConvert.DeserializeObject<accesstoken>(srcString);
                    return at;
                }
            }
            catch (Exception ex)
            {
                return new accesstoken { access_token = "000001" };
            }
        }
        private jsticket getTicket(string atoken)
        {
            if (!string.IsNullOrEmpty(_tt.ticket))
            {
                if (DateTime.Now.CompareTo(_tt.ticketlast.AddSeconds(_tt.ticketexpires_in - 100)) < 0)
                {
                    return new jsticket { ticket = _tt.ticket };
                }
            }
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", atoken);
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

                using (var restget = new HttpClient(handler))
                {
                    var response = restget.GetAsync(url).Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    var ti = JsonConvert.DeserializeObject<jsticket>(srcString);
                    return ti;
                }
            }
            catch (Exception ex)
            {
                return new jsticket { ticket = "000001" };
            }
        }
        private string getSignature(string ticket, string noncestr, string url, long stamp)
        {
            //   highlevel.redis.GetDatabase();

            SHA1 sha = SHA1.Create();

            //将mystr转换成byte[]

            ASCIIEncoding enc = new ASCIIEncoding();

            var str = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, noncestr, stamp, url);
            byte[] dataToHash = enc.GetBytes(str);

            //Hash运算

            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string

            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }
    }
}
