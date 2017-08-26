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

        private readonly blahContext _db1 = new blahContext();
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

        
      
        [Route("declarationSign")]
        [HttpPost]
        public commonresponse declarationSign([FromBody]declarationsignrequest input)
        {
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            var accinfo= highlevel.GetInfoByToken(Request.Headers);
           if(accinfo.status!= responseStatus.ok) return accinfo;   

        //  var acc=(access_idinfo)accinfo;

            if (!savePic(input.sign_pic, picType.declaration_sign, accinfo.Identity, accinfo.businessType))
                return new commonresponse { status = responseStatus.fileprocesserror };
            return new commonresponse { status = responseStatus.ok };
        }
        
        
        [Route("downloadpic")]
        [HttpGet]
        public commonresponse downloadpic(picType picType)
        {
           highlevel. LogRequest("downloadpic", "downloadpic", Request.HttpContext.Connection.RemoteIpAddress.ToString());

             var accinfo= highlevel.GetInfoByToken(Request.Headers);
           if(accinfo.status!= responseStatus.ok) return accinfo;   


            try
            {
                var fname = Path.Combine(_picpath, accinfo.Identity, accinfo.businessType.ToString(), picType + ".jpg");
                var bbytes = System.IO.File.ReadAllBytes(fname);
                var retstr = Convert.ToBase64String(bbytes);
                return new downloadresponse { status = responseStatus.ok, picture = retstr };
            }
            catch (Exception ex)
            {
                _log.LogError("{0}-{1}-{2}",DateTime.Now,"downloadpic",ex.Message);
                return new commonresponse { status = responseStatus.processerror,content=ex.Message };
            }

        }
        [Route("uploadpic")]
        [HttpPost]
        public commonresponse uploadpic([FromBody]uploadpicrequest input)
        {
           highlevel. LogRequest("uploadpic", "uploadpic", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
              var accinfo= highlevel.GetInfoByToken(Request.Headers);
           if(accinfo.status!= responseStatus.ok) return accinfo;   

            if (!savePic(input.picture, input.picType, accinfo.Identity, accinfo.businessType))
                return new commonresponse { status = responseStatus.fileprocesserror };

            _db1.Businesspic.Add(new Businesspic
            {
                Identity = accinfo.Identity,
                Businesstype = (short)accinfo.businessType,
                Pictype = (short)input.picType,
                Uploaded = true
            });
            _db1.SaveChanges();

            return new commonresponse { status = responseStatus.ok };
        }
        [Route("ChangeLicense")]
        [HttpPost]
        public commonresponse ChangeLicense([FromBody]changelicenserequest input)
        {
           highlevel. LogRequest("ChangeLicense", "ChangeLicense", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            var accinfo= highlevel.GetInfoByToken(Request.Headers);
           if(accinfo.status!= responseStatus.ok) return accinfo; 
           var identity=accinfo.Identity;
           var btype=accinfo.businessType;
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

      

    }
}
