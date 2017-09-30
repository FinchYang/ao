using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System
namespace wx.Controllers
{
    public class wechatController : Controller
    {
          public readonly ILogger<wechatController> _log;
            public wechatController(ILogger<wechatController> log)
        {
            _log = log;
        }
        [Route("wx")]
        [HttpGet]
        public string checkToken(string signature,string timestamp,string nonce,string echostr)
        {
            try{
            _log.LogWarning("signature={0}",signature);
_log.LogWarning("nonce={0}",nonce);
_log.LogWarning("timestamp={0}",timestamp);
_log.LogWarning("echostr={0}",echostr);
              SHA1 sha = SHA1.Create();
           var token="ycltoken";

            ASCIIEncoding enc = new ASCIIEncoding();

    string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp); //字典排序
            string str = string.Join("", ArrTmp);
         
         //   tmpStr = tmpStr.ToLower();

        //    var str = string.Format("nonce={0}&timestamp={1}&token={2}", nonce, timestamp, token);
            byte[] dataToHash = enc.GetBytes(str);
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            string hash = BitConverter.ToString(dataHashed);//.Replace("-", "");
_log.LogWarning("hash=",hash);
 hash = hash.ToLower();

            if(hash==signature) return echostr;}
            catch(Exception ex){
                _log.LogError("error:{0}",ex.Message);
            }
            return "";
        }
        //   private bool CheckSignature(string signature,string timestamp,string nonce,string Token)
        // {
        //     string[] ArrTmp = { Token, timestamp, nonce };
        //     Array.Sort(ArrTmp); //字典排序
        //     string tmpStr = string.Join("", ArrTmp);
        //     tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
        //     tmpStr = tmpStr.ToLower();
        //     if (tmpStr == signature)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }
 private string getSignature(string ticket, string noncestr, string url, long stamp)
        {
            //   highlevel.redis.GetDatabase();

            SHA1 sha = SHA1.Create();

            //将mystr转换成byte[]

            ASCIIEncoding enc = new ASCIIEncoding();

            var str = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, noncestr, stamp, url);
            byte[] dataToHash = enc.GetBytes(str);

            //Hash运算

            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string

            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
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
