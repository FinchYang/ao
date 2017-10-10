using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc104.models;
using Microsoft.Extensions.Logging;
using encm.cars;

namespace mvc104.Controllers
{
    public class HomeController : Controller
    {
        public readonly ILogger<HomeController> _log;
        public HomeController(ILogger<HomeController> log)
        {
            _log = log;
        }
        [Route("CompleteScrap")]
        [HttpGet]
        public commonresponse CompleteScrap(string id)
        {
            try
            {
                var enid = CryptographyHelpers.StudyEncrypt(id);
                using (var db = new carsContext())
                {
                    var ss = db.Carbusiness.FirstOrDefault(a => a.Businesstype == (short)businessType.scrap&&a.Identity==enid);

                    if(ss!=null)
                    {
                       db.Carbusinesshis.Add(new Carbusinesshis
                       {
                           Identity = ss.Identity,
                           Businesstype = ss.Businesstype,
                           Completed = true,
                           Time = ss.Finishtime,
                           Status = ss.Status,
                           Waittime = ss.Waittime,
                           Processtime = ss.Processtime,
                           Finishtime = ss.Finishtime,
                           Exporttime = ss.Exporttime,
                           Cartype = ss.Cartype,
                           Platetype = ss.Platetype,
                           Scrapplace = ss.Scrapplace,
                           Reason = ss.Reason
                       });
                        db.SaveChanges();
                    }
                }
                Task.Run(() => highlevel.LogRequest("CompleteScrap",
                       "CompleteScrap", Request.HttpContext.Connection.RemoteIpAddress.ToString(), 0));
                return new ScrapListRes { status = responseStatus.ok};
            }
            catch (Exception ex)
            {
                _log.LogError("{0}-{1}-{2}", DateTime.Now, "CompleteScrap", ex.Message);
                return new commonresponse { status = responseStatus.processerror, content = ex.Message };
            }
        }
        [Route("getScrapTodoList")]
        [HttpGet]
        public commonresponse getScrapTodoList()
        {
            try
            {
                var scraps = new List<Scrap>();
                using (var db=new carsContext())
                {
                  var ss=  db.Carbusiness.Where(a => a.Businesstype == (short)businessType.scrap);
                   
                    foreach(var s in ss)
                    {
                        var user = db.Caruser.FirstOrDefault(a => a.Identity == s.Identity);
                        if (user == null) continue;
                        scraps.Add(new Scrap
                        {
                            id= CryptographyHelpers.StudyDecrypt(s.Identity),
                            name=user.Name,
                            phone=user.Phone,
                            address=s.Postaddr,
                            cartype=(CarType)s.Cartype,
                            time=s.Time,
                        });
                    }
                }
                Task.Run(() => highlevel.LogRequest("getScrapTodoList",
                       "getScrapTodoList", Request.HttpContext.Connection.RemoteIpAddress.ToString(), 0));
                return new ScrapListRes { status = responseStatus.ok ,scraps=scraps};
            }
            catch (Exception ex)
            {
                _log.LogError("{0}-{1}-{2}", DateTime.Now, "getScrapTodoList", ex.Message);
                return new commonresponse { status = responseStatus.processerror, content = ex.Message };
            }
        }

        public IActionResult chart()
        {
            //   ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
