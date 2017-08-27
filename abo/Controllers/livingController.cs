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

        private readonly string facepath = "face";
        private readonly string residencepicturepath = "residentpicture";
        private readonly blahContext _db1 = new blahContext();

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
            highlevel.infolog(_log, "FaceCompare", input.image.Length.ToString());
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            if (string.IsNullOrEmpty(input.image))
            {
                return new commonresponse { status = responseStatus.imageerror };
            }
            var fp = Path.Combine(facepath, accinfo.Identity);
            if (!Directory.Exists(fp)) Directory.CreateDirectory(fp);
            var now = DateTime.Now;
            var fbase = string.Format("{0}-{1}-{2}-{3}-{4}-{5}.jpg", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var fname = Path.Combine(fp, fbase);
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
                var api_id = "9a4c8ff73d6642d886c537403a0a736d";
                var api_secret = "d5f2e07d025b4bc8bdc8e4774f904fbf";

                var ret = living(api_id, api_secret, fname);
                _log.LogInformation("living check ret={0}", ret);
                var retsta = JsonConvert.DeserializeObject<okcheck>(ret);
                if (retsta.status != "OK")
                {
                    return new commonresponse { status = responseStatus.livingerror, content = ret };
                }
                var retok = JsonConvert.DeserializeObject<okcheck2>(ret);
                var score = double.Parse(retok.score);
                if (score >= 0.98)
                {
                    return new commonresponse { status = responseStatus.livingerror, content = retok.score };
                }

                var theuser = _db1.Aouser.FirstOrDefault(c => c.Identity == accinfo.Identity);
                if (theuser == null)
                {
                    return new commonresponse { status = responseStatus.nouser, content = accinfo.Identity };
                }
                if (string.IsNullOrEmpty(theuser.Photofile))
                {
                    return new commonresponse { status = responseStatus.residencepictureerror };
                }
                var history =Path.Combine(residencepicturepath, theuser.Photofile+ ".jpg");

                highlevel.infolog(_log,"historpic",fname);
                var rettwo = CompareWitdIdFace(api_id, api_secret, fname, history);
                var twoc = JsonConvert.DeserializeObject<okcheck>(rettwo);
                if (twoc.status != "OK")
                {
                    return new commonresponse { status = responseStatus.compareerror, content = rettwo };
                }
                var twook = JsonConvert.DeserializeObject<okchecktwo>(rettwo);
                var confidence = double.Parse(twook.confidence);
                if (confidence <= 0.78)
                {
                    return new commonresponse { status = responseStatus.compareerror, content = twook.confidence };
                }
                return new commonresponse { status = responseStatus.ok, content = twook.confidence };
            }
            catch (Exception ex)
            {
                _log.LogInformation("error: {0}", ex);
                return new commonresponse { status = responseStatus.fileprocesserror };
            }
        }
        private string CompareWitdIdFace(string api_id, string api_secret, string path, string historypath)
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

        //     [Route("livingbody")]
        // [HttpPost]
        // public commonresponse livingbody()
        // {         
        //     try
        //     {
        //         var fname = @"d:\ycl.jpg";
        //         // var req=new livingbodyrequest(){
        //         var     api_id="9a4c8ff73d6642d886c537403a0a736d";
        //         var     api_secret="d5f2e07d025b4bc8bdc8e4774f904fbf";
        //         //     file=System.IO.File.ReadAllBytes(fname)
        //         // };

        //     //     var bbytes=System.IO.File.ReadAllBytes(fname);
        //     //     var str64=Convert.ToBase64String(bbytes);
        //     //      var req=new livingbodyrequest2(){
        //     //         api_id="9a4c8ff73d6642d886c537403a0a736d",
        //     //         api_secret="d5f2e07d025b4bc8bdc8e4774f904fbf",
        //     //         file=str64
        //     //     };
        //     //    var theUrl="https://cloudapi.linkface.cn/hackness/selfie_hack_detect";
        //    //    var ret= SendRestHttpClientRequest(theUrl,JsonConvert.SerializeObject(req));
        //    var ret=living(api_id,api_secret,fname);
        //        _log.LogInformation("ret={0}",ret);
        //         return new commonresponse { status = responseStatus.ok ,content=ret};
        //     }
        //     catch (Exception ex)
        //     {
        //         _log.LogInformation("error: {0}", ex);
        //         return new commonresponse { status = responseStatus.fileprocesserror };
        //     }

        // }


    }
}
