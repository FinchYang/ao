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
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Data;
using mvc104.abm;
using enabo;
using encm.cars;

namespace mvc104.Controllers
{
    public class examController : Controller
    {
        public readonly ILogger<examController> _log;

        private readonly enaboContext _db1 = new enaboContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db1.Dispose();
            }
            base.Dispose(disposing);
        }
        public examController(ILogger<examController> log)
        {
            _log = log;
        }   
      
        public class ssresponse : commonresponse
        {
            public aboss ongoing { get; set; } = new aboss();
            public aboss completed { get; set; } = new aboss();
            public string onhtml { get; set; }
            public string comhtml { get; set; }
        }
        public class aballresponse : commonresponse
        {          
            public List<values> values { get; set; }
              public List<labels> labels { get; set; }
        }
        public class aboss
        {
            public Dictionary<int, int> bcount = new Dictionary<int, int>();
            public int Total { get; set; } = 0;
        }
        public class aaa{
            public int count { get; set; }
            public string day { get; set; }
        }
         public class labels{
            public string label { get; set; }
        }
          public class values{
            public string value { get; set; }
        }
        [Route("CheckBorder")]
        [HttpGet]
        public aballresponse CheckBorder()
        {
            var ret = new aballresponse { status = 0, values = new List<values>(), labels = new List<labels>() };
            try
            {
                using (var abdb = new mvc104.abm.studyinContext())
                {
                    var ah = abdb.History.Select(ab => ab.Finishdate).ToList();
                    var aaaaa = from one in ah
                                group one by one.ToString("yyyy-MM-dd") into onegroup
                                orderby onegroup.Key descending
                                select new aaa { day = onegroup.Key, count = onegroup.Count() };
                    foreach (var cc in aaaaa)
                    {
                        ret.labels.Add(new labels { label = cc.day });
                        ret.values.Add(new values { value = cc.count.ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                ret.content += ex.Message;
            }
            return ret;
        }
        [Route("AbOkDailyCount")]
        [HttpGet]
        public aballresponse AbOkDailyCount()
        {          
            var ret = new aballresponse { status = 0 ,values=new List<values>() ,labels=new List<labels>()};
            try
            {
                using (var abdb = new mvc104.abm.studyinContext())
                {
                    var ah = abdb.History.Select(ab =>ab.Finishdate).ToList();
                    var aaaaa = from one in ah
                            group one by one.ToString("yyyy-MM-dd") into onegroup
                            orderby onegroup.Key descending
                            select new aaa {day= onegroup.Key,count= onegroup.Count() };
                         //   var  memday=DateTime.Parse( aaaaa.First().day);
                   foreach(var cc in aaaaa){
                    //    if(DateTime.Parse(cc.day).AddDays(-1).CompareTo(memday)>0){

                    //    }
                       ret.labels.Add(new labels{label=cc.day});
                         ret.values.Add(new values{value=cc.count.ToString()});
                   }          
                }
            }
            catch (Exception ex)
            {
                ret.content += ex.Message;
            }
            
          
            return ret;
        }

            [Route("AbUsageAmount")]
        [HttpGet]
        public aballresponse AbUsageAmount()
        {          
            var ret = new aballresponse { status = 0 ,values=new List<values>() ,labels=new List<labels>()};
            try
            {
                using (var abdb = new mvc104.abm.studyinContext())
                {
                    var ah = abdb.Request.Where(a =>a.Method.Contains("InspectPostStudyStatus")).Select(b =>b.Time).ToList();
                    var aaaaa = from one in ah
                            group one by one.ToString("yyyy-MM-dd") into onegroup
                                orderby onegroup.Key descending
                                select new aaa {day= onegroup.Key,count= onegroup.Count() };
                   foreach(var cc in aaaaa){
                       ret.labels.Add(new labels{label=cc.day});
                         ret.values.Add(new values{value=cc.count.ToString()});
                   }                 
                }
            }
            catch (Exception ex)
            {
                ret.content += ex.Message;
            }            
          
            return ret;
        }
        [Route("carStatistics")]
        [HttpGet]
        public string carStatistics()
        {
            var start = DateTime.Now.AddYears(-100);
            var end = DateTime.Now;
            var onhtml = string.Empty;
            var busicount = 0;
            var combusi = 0;
            var userscount = 0;
            var success = 0;
            var intebusi = 0;
            try
            {
                using(var cardb=new carsContext())
                {
                    userscount = cardb.Caruser.Count();
                    busicount = cardb.Carbusiness.Count();
                    intebusi = cardb.Carbusiness.Count(b => b.Integrated == true);
                    combusi = cardb.Carbusinesshis.Count();
                    success = cardb.Carbusinesshis.Count(a => a.Completed == false);
                }              
            }
            catch (Exception ex)
            {
            }

            onhtml += "<li>进行中业务量: " + busicount + "</li>";
            onhtml += "<li>进行中完整业务量: " + intebusi + "</li>";
            onhtml += "<li>办结业务量: " + combusi + "</li>";
            onhtml += "<li>成功办结业务量: " + success + "</li>";
            onhtml += "<li>登录用户总数: " + userscount + "</li>";

            Response.ContentType = "text/event-stream";
            return "data:" + onhtml + "\n\n";
        }
        [Route("aboallStatistics")]
        [HttpGet]
        public string aboallStatistics()
        {
            var start = DateTime.Now.AddYears(-100);
            var end = DateTime.Now;
            var onhtml = string.Empty;
            var busicount = 0;
            var combusi = 0;
            var userscount = 0;
            var success=0;
            var  intebusi=0;
            try
            {
               userscount=_db1.Aouser.Count();
               busicount=_db1.Business.Count();
                intebusi=_db1.Business.Count(b =>b.Integrated==true);
               combusi=_db1.Businesshis.Count();
                success=_db1.Businesshis.Count(a =>a.Completed==false);
            }
            catch (Exception ex)
            {
            }

            onhtml += "<li>进行中业务量: " + busicount + "</li>";
              onhtml += "<li>进行中完整业务量: " + intebusi + "</li>";
            onhtml += "<li>办结业务量: " + combusi + "</li>";
            onhtml += "<li>成功办结业务量: " + success + "</li>";
            onhtml += "<li>登录用户总数: " + userscount + "</li>";
          
             Response.ContentType = "text/event-stream";
            return "data:"+onhtml + "\n\n";
        }
        [Route("searchStatistics")]
        [HttpGet]
        public ssresponse searchStatistics(string startdate, string enddate)
        {
            var start = DateTime.Now.AddYears(-100);
            var end = DateTime.Now;
            if (!DateTime.TryParse(startdate, out start))
            {
                return new ssresponse { status = responseStatus.startdateerror, content = responseStatus.startdateerror.ToString() };
            }
            if (!DateTime.TryParse(enddate, out end))
            {
                return new ssresponse { status = responseStatus.enddateerror, content = responseStatus.enddateerror.ToString() };
            }
            var ret = new ssresponse { status = 0 };
            _log.LogInformation("start is={0},end={1}", start, end);
            var onhtml = string.Empty;
            var comhtml = string.Empty;
            foreach (int a in Enum.GetValues(typeof(businessType)))
            {
                try
                {
                    if (a == 0) continue;
                    var cc = _db1.Business.Count(i => i.Businesstype == a && i.Integrated == true
                    && i.Time.CompareTo(start) >= 0 && i.Time.CompareTo(end) <= 0);
                    _log.LogInformation("cc is={0}", cc);
                    ret.ongoing.bcount.Add(a, cc);
                    onhtml += "<li>" + global.statistics[(businessType)a] + ": " + cc + "</li>";
                    ret.ongoing.Total += cc;

                    var ccc = _db1.Businesshis.Count(i => i.Businesstype == a
                    && i.Time.CompareTo(start) >= 0 && i.Time.CompareTo(end) <= 0);
                    ret.completed.bcount.Add(a, ccc);
                    comhtml += "<li>" + global.statistics[(businessType)a] + ": " + ccc + "</li>";
                    ret.completed.Total += ccc;
                }
                catch (Exception ex)
                {
                    ret.content += ex.Message;
                }
            }
            onhtml += "<li>" + global.statistics[(businessType)0] + ": " + ret.ongoing.Total + "</li>";
            comhtml += "<li>" + global.statistics[(businessType)0] + ": " + ret.completed.Total + "</li>";
            ret.onhtml = onhtml;
            ret.comhtml = comhtml;
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he,
                 "searchStatistics", Request.HttpContext.Connection.RemoteIpAddress.ToString()));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return ret;
        }
        public class abssresponse : commonresponse
        {
            public string onhtml { get; set; }
        }
      
        [Route("abStatistics")]
        [HttpGet]
        public abssresponse abStatistics(string startdate, string enddate)
        {
            var start = DateTime.Now.AddYears(-100);
            var end = DateTime.Now;
            if (!DateTime.TryParse(startdate, out start))
            {
                return new abssresponse { status = responseStatus.startdateerror, content = responseStatus.startdateerror.ToString() };
            }
            if (!DateTime.TryParse(enddate, out end))
            {
                return new abssresponse { status = responseStatus.enddateerror, content = responseStatus.enddateerror.ToString() };
            }
            var ret = new abssresponse { status = 0 };
            var onhtml = string.Empty;
            var hiscount = 0;
            var allreq = 0;
            var usecount = 0;
            try
            {
                using (var abdb = new mvc104.abm.studyinContext())
                {
                    hiscount = abdb.History.Count(a => a.Finishdate.CompareTo(start) >= 0 && a.Finishdate.CompareTo(end) <= 0);
                    allreq = abdb.Request.Count(a => a.Time.CompareTo(start) >= 0 && a.Time.CompareTo(end) <= 0);
                    usecount = abdb.Request.Count(a => !a.Method.Contains("LoginAndQuery") && a.Time.CompareTo(start) >= 0 && a.Time.CompareTo(end) <= 0);
                }
            }
            catch (Exception ex)
            {
                ret.content += ex.Message;
            }

            onhtml += "<li>学习完成量: " + hiscount + "</li>";
            onhtml += "<li>访问量: " + allreq + "</li>";
            onhtml += "<li>使用量: " + usecount + "</li>";
            ret.onhtml = onhtml;
            try
            {
                var he = Request.Host.ToString();
                foreach (var a in Request.Headers)
                {
                    he += "--" + a.Key + "=" + a.Value;
                }
                Task.Run(() => highlevel.LogRequest(he + startdate + enddate,
                 "abStatistics", Request.HttpContext.Connection.RemoteIpAddress.ToString()));
            }
            catch (Exception ex) { _log.LogError("dblog error:", ex); }
            return ret;
        }
         [Route("abStatistics1")]
        [HttpGet]
        public string abStatistics1()
        {
            var start = DateTime.Now.AddYears(-100);
            var end = DateTime.Now;
            var onhtml = string.Empty;
            var hiscount = 0;
            var allreq = 0;
            var usecount = 0;
            var users=0;
            try
            {
                using (var abdb = new mvc104.abm.studyinContext())
                {
                    users=abdb.User.Count(c =>c.Inspect=="1"&&c.Drugrelated=="0");
                    hiscount = abdb.History.Count(a => a.Finishdate.CompareTo(start) >= 0 && a.Finishdate.CompareTo(end) <= 0);
                    allreq = abdb.Request.Count(a => a.Time.CompareTo(start) >= 0 && a.Time.CompareTo(end) <= 0);
                    usecount = abdb.Request.Count(a => !a.Method.Contains("LoginAndQuery") && a.Time.CompareTo(start) >= 0 && a.Time.CompareTo(end) <= 0);
                }
            }
            catch (Exception ex)
            {
            }

            onhtml += "<li>学习完成量: " + hiscount + "</li>";
            onhtml += "<li>访问量: " + allreq + "</li>";
            onhtml += "<li>使用量: " + usecount + "</li>";
          onhtml += "<li>今日可学习用户数: " + users + "</li>";
             Response.ContentType = "text/event-stream";
            return "data:"+onhtml + "\n\n";
        }
    }
}
