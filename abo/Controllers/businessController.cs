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
    public class businessController : Controller
    {
        public readonly ILogger<businessController> _log;

        private readonly blahContext _db1 = new blahContext();


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                _db1.Dispose();
            }
            base.Dispose(disposing);
        }
        public businessController(ILogger<businessController> log)
        {
            _log = log;
        }


        [Route("updateinfo")]
        [HttpPost]
        public commonresponse updateinfo([FromBody]updateinforequest input)
        {
            highlevel.LogRequest("updateinfo", "updateinfo", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return new commonresponse { status = responseStatus.requesterror };
            }
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            if (string.IsNullOrEmpty(input.postaddr))
            {
                return new commonresponse { status = responseStatus.postaddrerror };
            }

            try
            {
                var theuser = _db1.User.FirstOrDefault(i => i.Identity == accinfo.Identity);
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

        [Route("postaddr")]
        [HttpPost]
        public commonresponse postaddr([FromBody]postaddrrequest input)
        {
            highlevel.LogRequest("postaddr", "postaddr", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (input == null)
            {
                return highlevel.commonreturn(responseStatus.requesterror);
            }
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            if (string.IsNullOrEmpty(input.postaddr))
            {
                return highlevel.commonreturn(responseStatus.postaddrerror);
            }
            // if (string.IsNullOrEmpty(input.acceptingplace))
            // {
            //     return highlevel.commonreturn(responseStatus.acceptingplaceerror);
            // }
            try
            {
                var theuser = _db1.Business.FirstOrDefault(i => i.Identity == accinfo.Identity && i.Businesstype == (short)accinfo.businessType);
                if (theuser == null)
                {
                    return highlevel.commonreturn(responseStatus.iderror);
                }
                theuser.Postaddr = input.postaddr;
                
                if (!string.IsNullOrEmpty(input.acceptingplace))
                theuser.Acceptingplace = input.acceptingplace;

                 if (!string.IsNullOrEmpty(input.quasiDrivingLicense))
                theuser.QuasiDrivingLicense = input.quasiDrivingLicense;
                _db1.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return highlevel.commonreturn(responseStatus.dberror);
            }
            return highlevel.commonreturn(responseStatus.ok);
        }

        [Route("validatephone")]
        [HttpGet]
        public commonresponse validatephone(string phone)
        {
           // highlevel.LogRequest("postaddr", "postaddr", Request.HttpContext.Connection.RemoteIpAddress.ToString());
            if (string.IsNullOrEmpty(phone))
            {
                return highlevel.commonreturn(responseStatus.phoneerror);
            }
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            try
            {
                var theuser = _db1.Business.FirstOrDefault(i => i.Identity == accinfo.Identity && i.Businesstype == (short)accinfo.businessType);
                if (theuser == null)
                {
                    return highlevel.commonreturn(responseStatus.iderror);
                }
                theuser.Postaddr = input.postaddr;
                
                if (!string.IsNullOrEmpty(input.acceptingplace))
                theuser.Acceptingplace = input.acceptingplace;

                 if (!string.IsNullOrEmpty(input.quasiDrivingLicense))
                theuser.QuasiDrivingLicense = input.quasiDrivingLicense;
                _db1.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return highlevel.commonreturn(responseStatus.dberror);
            }
            return highlevel.commonreturn(responseStatus.ok);
        }

    }
}
