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
using encm.msg;

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
        public class msgres:commonresponse
        {
            public string recap { get; set; }
            public int aball { get; set; }
            public int driverall { get; set; }
            public int carall { get; set; }
            public int absuccess { get; set; }
            public int driversuccess { get; set; }
            public int carsuccess { get; set; }
            public chartdata abchartdata { get; set; }
            public chartdata driverchartdata { get; set; }
            public chartdata carchartdata { get; set; }
            public int aballtoday { get; set; }
            public int driveralltoday { get; set; }
            public int caralltoday { get; set; }
            public int absuccesstoday { get; set; }
            public int driversuccesstoday { get; set; }
            public int carsuccesstoday { get; set; }
        }
        public class chartdata
        {
            public List<values> values { get; set; }
            public List<labels> labels { get; set; }
        }
        [Route("CheckRequestAb")]
        [HttpGet]
        public aballresponse CheckRequestAb()
        {
            var ret = new aballresponse { status = 0, values = new List<values>(), labels = new List<labels>() };
            try
            {
                using (var driverdb = new studyinContext())
                {
                    var ah = driverdb.Request.Where(ab => ab.Time.CompareTo(DateTime.Now.AddDays(-1)) >= 0).Select(a => a.Time);
                    var aaaaa = from one in ah
                                group one by one.ToString("yyyy-MM-dd HH") + "点" into onegroup
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
        [Route("CheckRequestCar")]
        [HttpGet]
        public aballresponse CheckRequestCar()
        {
            var ret = new aballresponse { status = 0, values = new List<values>(), labels = new List<labels>() };
            try
            {
                using (var driverdb = new carsContext())
                {
                    var ah = driverdb.Carslog.Where(ab => ab.Time.CompareTo(DateTime.Now.AddDays(-1)) >= 0).Select(a => a.Time);
                    var aaaaa = from one in ah
                                group one by one.ToString("yyyy-MM-dd HH") + "点" into onegroup
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
        [Route("CheckRequestDriver")]
        [HttpGet]
        public aballresponse CheckRequestDriver()
        {
            var ret = new aballresponse { status = 0, values = new List<values>(), labels = new List<labels>() };
            try
            {
                using (var driverdb = new enaboContext())
                {
                    var ah = driverdb.Request.Where(ab => ab.Time.CompareTo(DateTime.Now.AddDays(-1))>=0).Select(a =>a.Time);
                    var aaaaa = from one in ah
                                group one by one.ToString("yyyy-MM-dd HH")+"点" into onegroup
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
        [Route("CheckMsg")]
        [HttpGet]
        public msgres CheckMsg()
        {
            var ret = new msgres { status = 0,
                aball = 0, driverall = 0, carall = 0,
                absuccess = 0,
                carsuccess = 0,
                driversuccess = 0,
                aballtoday = 0,
                driveralltoday = 0,
                caralltoday = 0,
                absuccesstoday = 0,
                carsuccesstoday = 0,
                driversuccesstoday = 0,
                abchartdata = new chartdata { values = new List<values>(), labels = new List<labels>() },
                carchartdata = new chartdata { values = new List<values>(), labels = new List<labels>() },
                driverchartdata = new chartdata { values = new List<values>(), labels = new List<labels>() },
                recap = string.Empty
            };
            try
            {
                using (var msgdb = new messageContext())
                {
                   ret.aball= msgdb.Abmsg.Count();
                    ret.driverall = msgdb.Drivermsg.Count();
                    ret.carall = msgdb.Carmsg.Count();

                    ret.absuccess = msgdb.Abmsg.Count(c => c.Sendflag);
                    ret.carsuccess = msgdb.Carmsg.Count(c => c.Sendflag);
                    ret.driversuccess = msgdb.Drivermsg.Count(c => c.Sendflag);

                    var today = DateTime.Now;
                    var tod = new DateTime(today.Year, today.Month, today.Day);
                    ret.aballtoday = msgdb.Abmsg.Count(c=>c.Timestamp.CompareTo(tod)>=0);
                    ret.driveralltoday = msgdb.Drivermsg.Count(c => c.Timestamp.CompareTo(tod) >= 0);
                    ret.caralltoday = msgdb.Carmsg.Count(c => c.Timestamp.CompareTo(tod) >= 0);

                    ret.absuccesstoday = msgdb.Abmsg.Count(c => c.Sendflag
                    && c.Timestamp.CompareTo(tod) >= 0);
                    ret.carsuccesstoday = msgdb.Carmsg.Count(c => c.Sendflag && c.Timestamp.CompareTo(tod) >= 0);
                    ret.driversuccesstoday = msgdb.Drivermsg.Count(c => c.Sendflag && c.Timestamp.CompareTo(tod) >= 0);
                    {
                        var ab = msgdb.Abmsg.Where(c => c.Sendflag).Select(c => c.Timestamp);//.ToList();
                        var aaaaa = from one in ab
                                    group one by one.ToString("yyyy-MM-dd") into onegroup
                                    orderby onegroup.Key descending
                                    select new aaa { day = onegroup.Key, count = onegroup.Count() };
                        foreach (var cc in aaaaa)
                        {
                            ret.abchartdata.labels.Add(new labels { label = cc.day });
                            ret.abchartdata.values.Add(new values { value = cc.count.ToString() });
                        }
                    }
                    {
                        var ab = msgdb.Carmsg.Where(c => c.Sendflag).Select(c => c.Timestamp);//.ToList();
                        var aaaaa = from one in ab
                                    group one by one.ToString("yyyy-MM-dd") into onegroup
                                    orderby onegroup.Key descending
                                    select new aaa { day = onegroup.Key, count = onegroup.Count() };
                        foreach (var cc in aaaaa)
                        {
                            ret.carchartdata.labels.Add(new labels { label = cc.day });
                            ret.carchartdata.values.Add(new values { value = cc.count.ToString() });
                        }
                    }
                    {
                        var ab = msgdb.Drivermsg.Where(c => c.Sendflag).Select(c => c.Timestamp);//.ToList();
                        var aaaaa = from one in ab
                                    group one by one.ToString("yyyy-MM-dd") into onegroup
                                    orderby onegroup.Key descending
                                    select new aaa { day = onegroup.Key, count = onegroup.Count() };
                        foreach (var cc in aaaaa)
                        {
                            ret.driverchartdata.labels.Add(new labels { label = cc.day });
                            ret.driverchartdata.values.Add(new values { value = cc.count.ToString() });
                        }
                    }
                    ret.recap += "<li>车管总短信量: " + ret.carall + "</li>";
                    ret.recap += "<li>车管总成功量: " + ret.carsuccess + "</li>";
                    ret.recap += "<li>驾管总短信量: " + ret.driverall + "</li>";
                    ret.recap += "<li>驾管总成功量: " + ret.driversuccess + "</li>";
                    ret.recap += "<li>AB总短信量: " + ret.aball + "</li>";
                    ret.recap += "<li>AB总成功量: " + ret.absuccess + "</li>";
                    ret.recap += "<li>车管今日短信量: " + ret.caralltoday + "</li>";
                    ret.recap += "<li>车管今日成功量: " + ret.carsuccesstoday + "</li>";
                    ret.recap += "<li>驾管今日短信量: " + ret.driveralltoday + "</li>";
                    ret.recap += "<li>驾管今日成功量: " + ret.driversuccesstoday + "</li>";
                    ret.recap += "<li>AB今日短信量: " + ret.aballtoday + "</li>";
                    ret.recap += "<li>AB今日成功量: " + ret.absuccesstoday + "</li>";
                }
            }
            catch (Exception ex)
            {
                ret.content += ex.Message;
            }
            return ret;
        }
        [Route("CheckBorderDriver")]
        [HttpGet]
        public aballresponse CheckBorderDriver()
        {
            var ret = new aballresponse { status = 0, values = new List<values>(), labels = new List<labels>() };
            try
            {
                var getpath = "/home/endriver/ftp/get/back";
                var gt = new DirectoryInfo(getpath).GetFiles().Where(a => a.Name.Contains("over"));

                var aaaaa = from one in gt
                            group one by one.Name.Substring(0, 10) into onegroup
                            orderby onegroup.Key descending
                            select new aaa { day = onegroup.Key, count = onegroup.Count() };
                foreach (var cc in aaaaa)
                {
                    ret.labels.Add(new labels { label = cc.day });
                    ret.values.Add(new values { value = cc.count.ToString() });
                }

            }
            catch (Exception ex)
            {
                ret.content += ex.Message;
            }

            return ret;
        }
        [Route("CheckBorder")]
        [HttpGet]
        public aballresponse CheckBorder()
        {
            var ret = new aballresponse { status = 0, values = new List<values>(), labels = new List<labels>() };
            try
            {
                var getpath = "/home/carbusiness/ftp/get/back";
                var gt = new DirectoryInfo(getpath).GetFiles().Where(a => a.Name.Contains("over"));

                var aaaaa = from one in gt
                            group one by one.Name.Substring(0, 10) into onegroup
                            orderby onegroup.Key descending
                            select new { day = onegroup.Key, count = onegroup.Count() };
                foreach (var cc in aaaaa)
                {
                    ret.labels.Add(new labels { label = cc.day });
                    ret.values.Add(new values { value = cc.count.ToString() });
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
                    var ah = abdb.History.Select(ab =>ab.Finishdate);
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
                    var ah = abdb.Request.Where(a =>a.Method.Contains("InspectPostStudyStatus")).Select(b =>b.Time);
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
            return onhtml ;
            //  Response.ContentType = "text/event-stream";
            //  return "data:" + onhtml + "\n\n";
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
            return  onhtml ;
            //   Response.ContentType = "text/event-stream";
            //  return "data:"+onhtml + "\n\n";
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
        public abssresponse abStatistics(string startdate="2000-1-1", string enddate="2222-2-2")
        {
            var start = DateTime.Now.AddYears(-100);
            var end = DateTime.Now;
            if (!DateTime.TryParse(startdate, out start))
            {
               // return new abssresponse { status = responseStatus.startdateerror, content = responseStatus.startdateerror.ToString() };
            }
            if (!DateTime.TryParse(enddate, out end))
            {
               // return new abssresponse { status = responseStatus.enddateerror, content = responseStatus.enddateerror.ToString() };
            }
            var ret = new abssresponse { status = 0 ,onhtml="something happened"};
            var onhtml = string.Empty;
            var hiscount = 0;
            var allreq = 0;
            var usecount = 0;
            var users = 0;
            try
            {
                using (var abdb = new mvc104.abm.studyinContext())
                {
                    users = abdb.User.Count(c => c.Inspect == "1" && c.Drugrelated == "0");
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
            onhtml += "<li>今日可学习用户数: " + users + "</li>";
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
    }
}
