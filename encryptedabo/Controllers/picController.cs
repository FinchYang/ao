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
    public class picController : Controller
    {
        public readonly ILogger<picController> _log;
        private readonly aboContext _db1 = new aboContext();
        static string _picpath = "pictures";

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db1.Dispose();
            }
            base.Dispose(disposing);
        }
        public picController(ILogger<picController> log)
        {
            _log = log;
        }
        

        [Route("downloadpic")]
        [HttpGet]
        public commonresponse downloadpic(picType picType)
        {
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            try
            {
                var fname = Path.Combine(_picpath, accinfo.photofile, accinfo.businessType.ToString(), picType + ".jpg");
              //  highlevel.infolog(_log, "downloadpic", fname);
                var bbytes = CryptographyHelpers.StudyFileDecrypt(fname);
                var retstr ="data:image/jpeg;base64," +Convert.ToBase64String(bbytes);
                try
                {
                    var he = Request.Host.ToString();
                    foreach (var a in Request.Headers)
                    {
                        he += "--" + a.Key + "=" + a.Value;
                    }
                    Task.Run(() => highlevel.LogRequest(he + accinfo.photofile + picType+accinfo.Identity,
                     "downloadpic", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
                }
                catch (Exception ex) { _log.LogError("dblog error:", ex); }
                return new downloadresponse { status = responseStatus.ok, picture = retstr };
            }
            catch (Exception ex)
            {
                _log.LogError("{0}-{1}-{2}", DateTime.Now, "downloadpic", ex.Message);
                return new commonresponse { status = responseStatus.processerror, content = ex.Message };
            }
        }
        [Route("uploadpic")]
        [HttpPost]
        public commonresponse uploadpic([FromBody]uploadpicrequest input)
        {
            //  highlevel.LogRequest("uploadpic", "uploadpic", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return highlevel.commonreturn(responseStatus.requesterror);
            }
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            _log.LogInformation("uploadpic-{3}: id={0},bt={1},pictype={2}",accinfo.Identity,accinfo.businessType,input.picType,DateTime.Now);
            if (!savePic(input.picture, input.picType, accinfo.photofile, accinfo.businessType))
                return highlevel.commonreturn(responseStatus.fileprocesserror);

            if (input.picType == picType.unknown)
            {
                return highlevel.commonreturn(responseStatus.pictypeerror);
            }
            if (accinfo.businessType == businessType.unknown)
            {
                return highlevel.commonreturn(responseStatus.businesstypeerror);
            }
            try
            {
                using (var ddbb = new aboContext())
                {
                    var already = ddbb.Businesspic.FirstOrDefault(i => i.Businesstype == (int)accinfo.businessType && i.Identity == accinfo.Identity && i.Pictype == (short)input.picType);
                    if (already == null)
                    {
                        var newpic = new Businesspic
                        {
                            Identity = accinfo.Identity,
                            Businesstype = (short)accinfo.businessType,
                            Pictype = (short)input.picType,
                            Uploaded = true,
                            Time = DateTime.Now
                        };
                        //  highlevel.infolog(_log, "uploadpic", JsonConvert.SerializeObject(newpic));
                        var ret = ddbb.Businesspic.Add(newpic);
                        //  highlevel.infolog(_log, "uploadpic88", JsonConvert.SerializeObject(ret.Entity));

                    }
                    else
                    {
                        already.Uploaded = true;
                        already.Time = DateTime.Now;
                    }
                    ddbb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                highlevel.errorlog(_log, "uploadpic", ex);
            }
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he+input.picType+accinfo.Identity,
                 "uploadpic", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
             _log.LogInformation("uploadpic-{3}: id={0},bt={1},pictype={2}---over",accinfo.Identity,accinfo.businessType,input.picType,DateTime.Now);
            return new commonresponse { status = responseStatus.ok };
        }
      
        private bool savePic(string picstr, picType picType, string identity, businessType btype)
        {
            try
            {
                var fpath = Path.Combine(_picpath, identity, btype.ToString());
                if (!Directory.Exists(fpath)) Directory.CreateDirectory(fpath);
                var fname = Path.Combine(fpath, picType + ".jpg");
                
                var index = picstr.IndexOf("base64,");
                System.IO.File.WriteAllBytes(fname, Convert.FromBase64String(CryptographyHelpers.StudyEncrypt(picstr.Substring(index + 7))));
            }
            catch (Exception ex)
            {
                _log.LogInformation("savePic error: {0}", ex);
                return false;
            }
            return true;
        }
    }
}
