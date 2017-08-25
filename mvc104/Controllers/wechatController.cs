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

        // static void Main(string[] args)
        // {
        //     var ret = Upload("9a4c8ff73d6642d886c537403a0a736d", "d5f2e07d025b4bc8bdc8e4774f904fbf", "C:\\Users\\xing\\Documents\\visual studio 2017\\Projects\\ConsoleApp1\\ConsoleApp1\\test.jpg");
        //     byte[] bytes = new byte[ret.Length];
        //     ret.Read(bytes, 0, (int)ret.Length);

        //     Console.WriteLine(System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length));
        // }

    public string SendRestHttpClientRequest(string url, string param)
        {
           
           // var url = string.Format("http://{0}/{1}", host, method);
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                using (var http = new HttpClient(handler))
                {
                    var content1 = new StringContent("api_id=9a4c8ff73d6642d886c537403a0a736d");
                     var content2 = new StringContent("api_secret=d5f2e07d025b4bc8bdc8e4774f904fbf");
                    // var fcontent=new MultipartContent( );
                    // fcontent.Add(content1 );
                    var content = new MultipartFormDataContent("----------------");
                    content.Add(content1);
                     content.Append(content2);
                     
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data");// 
                   // content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var response = http.PostAsync(url, content).Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    return srcString;
                }
            }
            catch (Exception ex)
            {
                return  "000001"+ex.Message;
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
