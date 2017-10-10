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
using enabo;
using encm.cars;

namespace mvc104.Controllers
{
    public class businessController : Controller
    {
        public readonly ILogger<businessController> _log;

        private readonly carsContext _db = new carsContext();
     //   private readonly enaboContext _db1 = new enaboContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            //    _db1.Dispose();
            }
            base.Dispose(disposing);
        }
        public businessController(ILogger<businessController> log)
        {
            _log = log;
        }
        [Route("again")]
        [HttpGet]
        public commonresponse again()
        {
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            try
            {
                var theuser = _db.Carbusiness.FirstOrDefault(i => i.Identity == accinfo.Identity && i.Businesstype == (short)accinfo.businessType);
                if (theuser == null)
                {
                    return highlevel.commonreturn(responseStatus.iderror);
                }
                theuser.Integrated = false;
                 theuser.Status = (short)businessstatus.unknown;
                 theuser.Exporttime=new DateTime(2000,1,1);
                var reason = string.Empty;
                if (!string.IsNullOrEmpty(theuser.Reason)) reason = theuser.Reason;
                _db.Carbusinesshis.Add(new Carbusinesshis
                {
                    Identity = theuser.Identity,
                    Businesstype = theuser.Businesstype,
                    Completed = true,
                    Time = theuser.Finishtime,
                    Status = theuser.Status,
                    Waittime = theuser.Waittime,
                    Processtime = theuser.Processtime,
                    Finishtime = theuser.Finishtime,
                    Exporttime = theuser.Exporttime,
                    Cartype=theuser.Cartype,
                    Platetype=theuser.Platetype,
                    Scrapplace=theuser.Scrapplace,
                    Reason = reason
                });
               
                var pics = _db.Carbusinesspic.Where(i => i.Identity == accinfo.Identity && i.Businesstype == (short)accinfo.businessType);
                foreach (var p in pics)
                {
                    p.Uploaded = false;
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return highlevel.commonreturn(responseStatus.dberror);
            }
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he + accinfo.Identity,
                 "again", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return highlevel.commonreturn(responseStatus.ok);
        }
        
  
      
       
        [Route("getaddr")]
        [HttpGet]
        public commonresponse getaddr()
        {
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;
            var ret = new getaddrresponse { status = responseStatus.ok };
            try
            {
                var addrbusi = _db.Carbusiness.FirstOrDefault(i => i.Identity == accinfo.Identity && i.Businesstype == (short)accinfo.businessType);
                if (addrbusi == null)
                {
                    return highlevel.commonreturn(responseStatus.iderror);
                }
                 Task.Run(() => highlevel.LogRequest( accinfo.Identity+accinfo.businessType+JsonConvert.SerializeObject(addrbusi),
                 "getaddr", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
                if (!string.IsNullOrEmpty(addrbusi.Postaddr))
                    ret.detailedAddress = addrbusi.Postaddr;
                if (!string.IsNullOrEmpty(addrbusi.Platenumber1))
                    ret.plateNumber1 = addrbusi.Platenumber1;
                if (!string.IsNullOrEmpty(addrbusi.Platenumber2))
                    ret.plateNumber2 = addrbusi.Platenumber2;
                ret.plateType = (PlateType)addrbusi.Platetype;
                ret.scrapPlace = (ScrapPlace)addrbusi.Scrapplace;
                ret.carType = (CarType)addrbusi.Cartype;
                if (!string.IsNullOrEmpty(addrbusi.Province))
                    ret.province = addrbusi.Province;
                if (!string.IsNullOrEmpty(addrbusi.City))
                    ret.city = addrbusi.City;
                if (!string.IsNullOrEmpty(addrbusi.County))
                    ret.county = addrbusi.County;
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return highlevel.commonreturn(responseStatus.dberror);
            }
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he + accinfo.Identity+accinfo.businessType+JsonConvert.SerializeObject(ret),
                 "getaddr", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return ret;
        }

        [Route("postaddr")]
        [HttpPost]
        public commonresponse postaddr([FromBody]Postaddrrequest input)
        {
            if (input == null)
            {
                return highlevel.commonreturn(responseStatus.requesterror);
            }
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            if (string.IsNullOrEmpty(input.detailedAddress))
            {
                return highlevel.commonreturn(responseStatus.postaddrerror);
            }
            try
            {
                var theuser = _db.Carbusiness.FirstOrDefault(i => i.Identity == accinfo.Identity && i.Businesstype == (short)accinfo.businessType);
                if (theuser == null)
                {
                    return highlevel.commonreturn(responseStatus.iderror);
                }
                theuser.Postaddr = input.detailedAddress;
                theuser.Scrapplace =(short) input.scrapPlace;
                theuser.Cartype = (short)input.carType;
                theuser.Platenumber1 = input.plateNumber1;
                theuser.Platenumber2 = input.plateNumber2;
                theuser.Platetype = (short)input.plateType;
                if (!string.IsNullOrEmpty(input.acceptingplace))
                    theuser.Acceptingplace = input.acceptingplace;
                if (!string.IsNullOrEmpty(input.province))
                    theuser.Province = input.province;
                if (!string.IsNullOrEmpty(input.city))
                    theuser.City = input.city;
                if (!string.IsNullOrEmpty(input.county))
                    theuser.County = input.county;

                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return highlevel.commonreturn(responseStatus.dberror);
            }
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he + accinfo.Identity + accinfo.businessType+JsonConvert.SerializeObject(input),
                 "postaddr", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return highlevel.commonreturn(responseStatus.ok);
        }

        [Route("sendvcode")]
        [HttpGet]
        public commonresponse sendvcode(string phone)
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
                var theuser = _db.Caruser.FirstOrDefault(i => i.Identity == accinfo.Identity);
                if (theuser == null)
                {
                    return highlevel.commonreturn(responseStatus.iderror);
                }
                var rd = new Random();
                var vcode = rd.Next(0, 999999).ToString("D6");
                theuser.Verificationcode = vcode;
                theuser.Newphone = phone;
                _db.SaveChanges();

                var a = new System.Diagnostics.Process();
                a.StartInfo.UseShellExecute = true;
                a.StartInfo.Arguments =
                string.Format(" {0} {1}", phone, vcode);
                a.StartInfo.FileName = "/home/carbusiness/bin/sendmsg";
                a.Start();
                a.WaitForExit();
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return highlevel.commonreturn(responseStatus.dberror);
            }
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he + accinfo.Identity + phone,
                 "sendvcode", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return highlevel.commonreturn(responseStatus.ok);
        }

        [Route("checkvcode")]
        [HttpGet]
        public commonresponse checkvcode(string vcode)
        {
            // highlevel.LogRequest("postaddr", "postaddr", Request.HttpContext.Coannection.RemoteIpAddress.ToString());
            if (string.IsNullOrEmpty(vcode))
            {
                return highlevel.commonreturn(responseStatus.vcodeerror);
            }
            var accinfo = highlevel.GetInfoByToken(Request.Headers);
            if (accinfo.status != responseStatus.ok) return accinfo;

            try
            {
                var theuser = _db.Caruser.FirstOrDefault(i => i.Identity == accinfo.Identity && i.Verificationcode == vcode);
                if (theuser == null)
                {
                    return highlevel.commonreturn(responseStatus.vcodeerror);
                }

                theuser.Phone = theuser.Newphone;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _log.LogError("db error:{0}", ex.Message);
                return highlevel.commonreturn(responseStatus.dberror);
            }
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he + accinfo.Identity,
                 "checkvcode", Request.HttpContext.Connection.RemoteIpAddress.ToString(), (short)accinfo.businessType));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return highlevel.commonreturn(responseStatus.ok);
        }
    }
}
