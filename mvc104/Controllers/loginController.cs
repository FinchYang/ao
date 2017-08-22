using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc104.models;

namespace mvc104.Controllers
{
    public class loginController : Controller
    {
        public readonly ILogger<loginController> _log;

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
        public loginresponse login(string name, string identify, string phone)
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
            var token = GetToken();
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
            var htoken = Request.Headers["token"].First();
            if (string.IsNullOrEmpty(htoken))
            {
                return new commonresponse { status = responseStatus.tokenerror };
            }
            if (string.IsNullOrEmpty(input.image))
            {
                return new commonresponse { status = responseStatus.imageerror };
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
            try
            {
                var fname = Path.GetTempFileName();
                var index = input.image.IndexOf("base64,");
                System.IO.File.WriteAllBytes(fname, Convert.FromBase64String(input.image.Substring(index + 7)));
            }
            catch (Exception ex)
            {
                _log.LogInformation("error: {0}", ex);
                return new commonresponse { status = responseStatus.fileprocesserror };
            }
            return new commonresponse { status = responseStatus.ok };
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
