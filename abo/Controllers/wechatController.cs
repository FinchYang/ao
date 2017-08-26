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
    public class wechatController : Controller
    {
        public readonly ILogger<wechatController> _log;

        static tokenticket _tt = new tokenticket();
        private readonly blahContext _db1 = new blahContext();
        static List<Ptoken> tokens = new List<Ptoken>();
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
        public wechatController(ILogger<wechatController> log)
        {
            _log = log;
        }


        private string living(string api_id, string api_secret, string path)
        {
            HttpContent apiId = new StringContent(api_id);
            HttpContent apiSecret = new StringContent(api_secret);
            HttpContent photo = new ByteArrayContent(System.IO. File.ReadAllBytes(path));
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

          [Route("getwxconfig")]
        [HttpGet]
        public commonresponse getwxconfig(string url)
        {
           var accinfo= highlevel.GetInfoByToken(Request.Headers);
           if(accinfo.status!= responseStatus.ok) return accinfo;         
            

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
            [Route("livingbody")]
        [HttpPost]
        public commonresponse livingbody()
        {         
            try
            {
                var fname = @"d:\ycl.jpg";
                // var req=new livingbodyrequest(){
                var     api_id="9a4c8ff73d6642d886c537403a0a736d";
                var     api_secret="d5f2e07d025b4bc8bdc8e4774f904fbf";
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
           var ret=living(api_id,api_secret,fname);
               _log.LogInformation("ret={0}",ret);
                return new commonresponse { status = responseStatus.ok ,content=ret};
            }
            catch (Exception ex)
            {
                _log.LogInformation("error: {0}", ex);
                return new commonresponse { status = responseStatus.fileprocesserror };
            }
           
        }
        private string GetToken()
        {
            var seed = Guid.NewGuid().ToString("N");
            return seed;
        }
       
    }
}
