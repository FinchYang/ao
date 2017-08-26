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
    public class livingController : Controller
    {
        public readonly ILogger<livingController> _log;

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
        public livingController(ILogger<livingController> log)
        {
            _log = log;
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
                foreach (var a in global. tokens)
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
