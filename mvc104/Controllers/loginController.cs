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
        static string _picpath="pictures";
        class Ptoken
        {
            public string Identity { get; set; }
            public string Token { get; set; }
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
           private async void LogRequest(string content, string method = null, string ip = null,short businessType=0)
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
                using(var logdb=new blahContext()){
                logdb.Request.Add(new Request
                {
                    Content = dbtext,
                   Businesstype=businessType,
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
                        identity = a.Identity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return new commonresponse { status = responseStatus.tokenerror };
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
          
               if(!savePic(input.sign_pic,picType.declaration_sign,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
            return new commonresponse { status = responseStatus.ok };
        }
           [Route("uploadpic")]
        [HttpPost]
        public commonresponse uploadpic([FromBody]uploadpicrequest input)
        {
             LogRequest("uploadpic","uploadpic",Request.HttpContext.Connection.RemoteIpAddress.ToString());
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
                        identity = a.Identity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return new commonresponse { status = responseStatus.tokenerror };
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
            
               if(!savePic(input.picture,input.picType,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
            return new commonresponse { status = responseStatus.ok };
        }
            [Route("updateinfo")]
        [HttpPost]
        public commonresponse updateinfo([FromBody]updateinforequest input)
        {
             LogRequest("updateinfo","updateinfo",Request.HttpContext.Connection.RemoteIpAddress.ToString());
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
                        identity = a.Identity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
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
            
            try{
              var theuser = _db1.User.FirstOrDefault(i => i.Identity == identity);
            if (theuser == null)
            {
                return new commonresponse { status = responseStatus.iderror };
            }
            theuser.Postaddr=input.postaddr;
            _db1.SaveChanges();
            }catch(Exception ex){
                _log.LogError("db error:{0}",ex.Message);
                return new commonresponse { status = responseStatus.dberror };
            }
            return new commonresponse { status = responseStatus.ok };
        }
         [Route("ChangeLicense")]
        [HttpPost]
        public commonresponse ChangeLicense([FromBody]changelicenserequest input)
        {
             LogRequest("ChangeLicense","ChangeLicense",Request.HttpContext.Connection.RemoteIpAddress.ToString());
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
                        identity = a.Identity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return new commonresponse { status = responseStatus.tokenerror };
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
            if(input.lost) {
                if(!savePic(input.sign_pic,picType.sign_pic,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
            }
            else{
                 if(!savePic(input.license_pic,picType.driver,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
            }
             if(!savePic(input.id_back,picType.id_back,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
                  if(!savePic(input.id_front,picType.id_front,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
                  if(!savePic(input.id_inhand,picType.id_inhand,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
               if(!savePic(input.hukou_pic,picType.hukou_pic,identity))
                 return new commonresponse { status = responseStatus.fileprocesserror };
            return new commonresponse { status = responseStatus.ok };
        }
        private bool savePic(string picstr,picType picType,string identity){
             try
            {               
                var fpath=Path.Combine(_picpath,identity);
                if(!Directory.Exists(fpath)) Directory.CreateDirectory(fpath);
                 var fname = Path.Combine(fpath,identity+picType+".jpg");
                var index =picstr.IndexOf("base64,");
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
        public loginresponse login(string name, string identify, string phone,businessType businessType)
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
            _log.LogInformation("{3}-{0} from {1}, input is {2}", DateTime.Now, "login", Request.HttpContext.Connection.RemoteIpAddress.ToString() + HttpContext.Connection.RemoteIpAddress,
            identify + name + phone);

            var theuser = _db1.User.FirstOrDefault(i => i.Identity == identify);
            if (theuser == null)
            {
                return new loginresponse { status = responseStatus.iderror };
            }
            theuser.Name=name;
            theuser.Phone=phone;
            _db1.SaveChanges();

            var token = GetToken();
            var redisdb=highlevel.redis.GetDatabase();
        //    var memtoken= redisdb.StringGet(identify);
        //    if(memtoken=="nil") {
               redisdb.StringSet(identify,token);
         //  }
            var found = false;
            foreach (var a in tokens)
            {
                if (a.Identity == identify)
                {
                    a.Token = token;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                tokens.Add(new Ptoken { Identity = identify, Token = token });
            }
            LogRequest(name+phone+identify,"login",Request.HttpContext.Connection.RemoteIpAddress.ToString(),(short)businessType);
            return new loginresponse { status = responseStatus.ok, token = token };
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
                var inin=JsonConvert.SerializeObject(input);
                _log.LogInformation("input ={1},{0}",inin.Length);
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
                        identity = a.Identity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
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

            try
            {
                var fname = Path.GetTempFileName()+".jpg";
                var index = input.image.IndexOf("base64,");
                  _log.LogInformation("length: {0}", input.image.Length);
                System.IO.File.WriteAllBytes(fname, Convert.FromBase64String(input.image.Substring(index + 7)));
            }
            catch (Exception ex)
            {
                _log.LogInformation("error: {0}", ex);
                return new commonresponse { status = responseStatus.fileprocesserror };
            }
            return new commonresponse { status = responseStatus.ok };
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
                        identity = a.Identity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return new wxconfigresponse { status = responseStatus.tokenerror };
                }
            }
            catch (Exception ex)
            {
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
