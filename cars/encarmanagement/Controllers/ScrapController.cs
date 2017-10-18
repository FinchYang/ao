using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc104.models;
using Microsoft.Extensions.Logging;
using encm.cars;
using Newtonsoft.Json;

namespace mvc104.Controllers
{
    public class ScrapController : Controller
    {
        public readonly ILogger<ScrapController> _log;
        private readonly carsContext _db1 = new carsContext();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db1.Dispose();
            }
            base.Dispose(disposing);
        }
        public ScrapController(ILogger<ScrapController> log)
        {
            _log = log;
        }
        public class loginrequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        public class surequest
        {
            public string phone { get; set; }
            public string name { get; set; }
            public string password { get; set; }
        }
        [Route("scrapuserupdate")]
        [HttpPost]
        public commonresponse scrapuserupdate([FromBody] surequest inputRequest)
        {
            try
            {
                if (inputRequest == null)
                {
                    _log.LogInformation("login,{0}", responseStatus.requesterror);
                    return new ScrapLoginRes
                    {
                        status = responseStatus.requesterror
                    };
                }
                var token = Request.Headers["Token"].First();
                var found = false;
                var scrapplace = ScrapPlace.unknown;
                var user = string.Empty;
                foreach (var a in tokens)
                {
                    if (a.Token == token)
                    {
                        scrapplace = a.scrapPlace;
                        user = a.Identity;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return new commonresponse
                    {
                        status = responseStatus.tokenerror
                    };
                }
                var theuser = _db1.Operator.FirstOrDefault(async => async.Id == user);//|| async.Identity == cryptographicid);
                if (theuser == null)
                {
                    _log.LogInformation("login,{0}", responseStatus.nouser);
                    return new ScrapLoginRes
                    {
                        status = responseStatus.nouser
                    };
                }

                if (!string.IsNullOrEmpty(inputRequest.name)) theuser.Name = inputRequest.name;
                if (!string.IsNullOrEmpty(inputRequest.password)) theuser.Password = inputRequest.password;
                if (!string.IsNullOrEmpty(inputRequest.phone)) theuser.Phone = inputRequest.phone;
                _db1.SaveChanges();
                return new ScrapLoginRes
                {
                    status = 0,
                };
            }
            catch (Exception ex)
            {
                _log.LogError("login,{0}", ex);
                return new ScrapLoginRes
                {
                    status = responseStatus.processerror
                };
            }
        }
        [Route("scraplogin")]
        [HttpPost]
        public ScrapLoginRes scraplogin([FromBody] loginrequest inputRequest)
        {
            try
            {

                var input = JsonConvert.SerializeObject(inputRequest);
                //   LogRequest(input, "SignatureQuery", Request.HttpContext.Connection.RemoteIpAddress.ToString());

                if (inputRequest == null)
                {
                    _log.LogInformation("login,{0}", responseStatus.requesterror);
                    return new ScrapLoginRes
                    {
                        status = responseStatus.requesterror
                    };
                }
                _log.LogInformation("login,input={0},", input);//, Request.HttpContext.Connection.RemoteIpAddress);
                var allstatus = string.Empty;
                var identity = inputRequest.username;

                var theuser = _db1.Operator.FirstOrDefault(async => async.Id == identity);//|| async.Identity == cryptographicid);
                if (theuser == null)
                {
                    _log.LogInformation("login,{0}", responseStatus.nouser);
                    return new ScrapLoginRes
                    {
                        status = responseStatus.nouser
                    };
                }
                if (theuser.Password != inputRequest.password)

                {
                    _log.LogInformation("login,{0}", responseStatus.passerror);
                    return new ScrapLoginRes
                    {
                        status = responseStatus.passerror
                    };
                }
                //token process
                var toke1n = GetToken();
                var found = false;

                foreach (var a in tokens)
                {
                    if (a.Identity == identity)
                    {
                        a.Token = toke1n;
                        a.scrapPlace =(ScrapPlace) theuser.Scrapplace;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    tokens.Add(new Ptoken { Identity = identity, Token = toke1n, scrapPlace= (ScrapPlace)theuser.Scrapplace });
                }


                return new ScrapLoginRes
                {
                    status = 0,
                   token = toke1n,
                };
            }
            catch (Exception ex)
            {
                _log.LogError("login,{0}", ex);
                return new ScrapLoginRes
                {
                    status =responseStatus.processerror
                };
            }
        }
        static List<Ptoken> tokens = new List<Ptoken>();
        class Ptoken
        {
            public string Identity { get; set; }
            public string Token { get; set; }
            public ScrapPlace scrapPlace { get; set; }
        }
        private string GetToken()
        {
            var seed = Guid.NewGuid().ToString("N");
            return seed;
        }
        [Route("DeleteScrap")]
        [HttpGet]
        public commonresponse DeleteScrap(string id)
        {
            try
            {
                var token = Request.Headers["Token"].First();
                _log.LogInformation("pushmessage2,token is {0},{1},id", token, id);
                var found = false;
                var scrapplace = ScrapPlace.unknown;
                foreach (var a in tokens)
                {
                    if (a.Token == token)
                    {
                        scrapplace = a.scrapPlace;
                        found = true;
                        break;
                    }
                }
                _log.LogInformation("DeleteScrap,{0},,", 111);
                if (!found)
                {
                    _log.LogInformation("DeleteScrap,{0}", responseStatus.tokenerror);
                    return new commonresponse
                    {
                        status = responseStatus.tokenerror
                    };
                }
                var enid = CryptographyHelpers.StudyEncrypt(id);
                using (var db = new carsContext())
                {
                    var ss = db.Carbusiness.FirstOrDefault(a => a.Businesstype == (short)businessType.scrap 
                    &&a.Scrapplace==(short)scrapplace
                    && a.Identity == enid);

                    if (ss != null)
                    {
                        db.Carbusinesshis.Add(new Carbusinesshis
                        {
                            Identity = ss.Identity,
                            Businesstype = ss.Businesstype,
                            Completed = false,
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
                        db.Carbusiness.Remove(ss);
                        db.SaveChanges();
                    }
                }
                Task.Run(() => highlevel.LogRequest("DeleteScrap",
                       "DeleteScrap", Request.HttpContext.Connection.RemoteIpAddress.ToString(), 0));
                return new commonresponse { status = responseStatus.ok };
            }
            catch (Exception ex)
            {
                _log.LogError("{0}-{1}-{2}", DateTime.Now, "DeleteScrap", ex.Message);
                return new commonresponse { status = responseStatus.processerror, content = ex.Message };
            }
        }
        [Route("CompleteScrap")]
        [HttpGet]
        public commonresponse CompleteScrap(string id)
        {
            try
            {
                var token = Request.Headers["Token"].First();
                _log.LogInformation("pushmessage2,token is {0},{1},id", token, id);
                var found = false;
                var scrapplace = ScrapPlace.unknown;
                foreach (var a in tokens)
                {
                    if (a.Token == token)
                    {
                        scrapplace = a.scrapPlace;
                        found = true;
                        break;
                    }
                }
                _log.LogInformation("DeleteScrap,{0},,", 111);
                if (!found)
                {
                    _log.LogInformation("DeleteScrap,{0}", responseStatus.tokenerror);
                    return new commonresponse
                    {
                        status = responseStatus.tokenerror
                    };
                }
                var enid = CryptographyHelpers.StudyEncrypt(id);
                using (var db = new carsContext())
                {
                    var ss = db.Carbusiness.FirstOrDefault(a => a.Businesstype == (short)businessType.scrap
                      && a.Scrapplace == (short)scrapplace
                    && a.Identity==enid);

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
                        db.Carbusiness.Remove(ss);
                        db.SaveChanges();
                    }
                }
                Task.Run(() => highlevel.LogRequest("CompleteScrap",
                       "CompleteScrap", Request.HttpContext.Connection.RemoteIpAddress.ToString(), 0));
                return new commonresponse { status = responseStatus.ok};
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
                var token = Request.Headers["Token"].First();
                var found = false;
                var scrapplace = ScrapPlace.unknown;
                foreach (var a in tokens)
                {
                    if (a.Token == token)
                    {
                        scrapplace = a.scrapPlace;
                        found = true;
                        break;
                    }
                }
                _log.LogInformation("DeleteScrap,{0},,", 111);
                if (!found)
                {
                    _log.LogInformation("DeleteScrap,{0}", responseStatus.tokenerror);
                    return new commonresponse
                    {
                        status = responseStatus.tokenerror
                    };
                }
                var scraps = new List<Scrap>();
                using (var db=new carsContext())
                {
                  var ss=  db.Carbusiness.Where(a => a.Businesstype == (short)businessType.scrap
                    && a.Scrapplace == (short)scrapplace
                  );
                   
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
        [Route("getScrapDoneList")]
        [HttpGet]
        public commonresponse getScrapDoneList(string startdate="2000-1-1",string enddate="2222-2-2",CarType carType=CarType.unknown)
        {
            try
            {
                var token = Request.Headers["Token"].First();
                var found = false;
                var scrapplace = ScrapPlace.unknown;
                foreach (var a in tokens)
                {
                    if (a.Token == token)
                    {
                        scrapplace = a.scrapPlace;
                        found = true;
                        break;
                    }
                }
                _log.LogInformation("DeleteScrap,{0},,", 111);
                if (!found)
                {
                    _log.LogInformation("DeleteScrap,{0}", responseStatus.tokenerror);
                    return new commonresponse
                    {
                        status = responseStatus.tokenerror
                    };
                }
                var start = DateTime.Now;
                if(string.IsNullOrEmpty(startdate)||!DateTime.TryParse(startdate,out start))
                {
                    return new commonresponse { status = responseStatus.startdateerror, content = responseStatus.startdateerror.ToString() };
                }
                var end = DateTime.Now;
                if (string.IsNullOrEmpty(enddate) || !DateTime.TryParse(enddate, out end))
                {
                    return new commonresponse { status = responseStatus.enddateerror, content = responseStatus.enddateerror.ToString() };
                }
                var scraps = new List<Scrap>();
                using (var db = new carsContext())
                {
                    var ss = db.Carbusinesshis.Where(a => a.Businesstype == (short)businessType.scrap
                    &&a.Time.CompareTo(start)>=0
                    &&a.Time.CompareTo(end)<=0
                      && a.Scrapplace == (short)scrapplace
                    && a.Cartype==(short)carType);

                    foreach (var s in ss)
                    {
                        var user = db.Caruser.FirstOrDefault(a => a.Identity == s.Identity);
                        if (user == null) continue;
                        scraps.Add(new Scrap
                        {
                            id = CryptographyHelpers.StudyDecrypt(s.Identity),
                            name = user.Name,
                            phone = user.Phone,
                            address = s.Postaddr,
                            cartype = (CarType)s.Cartype,
                            time = s.Time,
                        });
                    }
                }
                Task.Run(() => highlevel.LogRequest("getScrapDoneList",
                       "getScrapDoneList", Request.HttpContext.Connection.RemoteIpAddress.ToString(), 0));
                return new ScrapListRes { status = responseStatus.ok, scraps = scraps };
            }
            catch (Exception ex)
            {
                _log.LogError("{0}-{1}-{2}", DateTime.Now, "getScrapDoneList", ex.Message);
                return new commonresponse { status = responseStatus.processerror, content = ex.Message };
            }
        }

    }
}
