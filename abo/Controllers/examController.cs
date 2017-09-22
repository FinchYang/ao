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
    public class examController : Controller
    {
        public readonly ILogger<examController> _log;

        private readonly aboContext _db1 = new aboContext();


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

        private string GetToken()
        {
            var seed = Guid.NewGuid().ToString("N");
            return seed;
        }

        private string exam()
        {

            var url = string.Format("http://jisujiakao.market.alicloudapi.com/driverexam/query?pagenum=1&pagesize=1&sort=rand&subject=1&type=A1",
            "wx774a9869c14f1647", "7f94f888c5e5c32bba9239230f46a827");
            try
            {
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

                using (var restget = new HttpClient(handler))
                {
                    var auth = new List<string>();
                    auth.Add("APPCODE a7686da69b354d78b1cd97b49ebd4490");
                    restget.DefaultRequestHeaders.Add("Authorization", auth);
                    var response = restget.GetAsync(url).Result;
                    string srcString = response.Content.ReadAsStringAsync().Result;

                    return srcString;
                }
            }
            catch (Exception ex)
            {
                highlevel.errorlog(_log, "exam", ex);
                return "000001";
            }
        }
        public class ssresponse : commonresponse
        {
            public aboss ongoing { get; set; } = new aboss();
            public aboss completed { get; set; } = new aboss();
        }
        public class aboss
        {
            public Dictionary<int, int> bcount = new Dictionary<int, int>();
            public int Total { get; set; } = 0;
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
            foreach (int a in Enum.GetValues(typeof(businessType)))
            {
                try
                {
                    var cc = _db1.Business.Count(i => i.Businesstype == a && i.Integrated == true
                    && i.Time.CompareTo(start) >= 0 && i.Time.CompareTo(end) <= 0);
                    _log.LogInformation("cc is={0}", cc);
                    ret.ongoing.bcount.Add(a, cc);
                    ret.ongoing.Total += cc;

                    var ccc = _db1.Businesshis.Count(i => i.Businesstype == a
                    && i.Time.CompareTo(start) >= 0 && i.Time.CompareTo(end) <= 0);
                    ret.completed.bcount.Add(a, ccc);
                    ret.completed.Total += ccc;
                }
                catch (Exception ex)
                {
                    ret.content += ex.Message;
                }
            }
            try
            {
                var he =  Request.Host.ToString();
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
    }
}
