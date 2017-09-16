using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Security.Cryptography;
namespace sync
{
    class Program
    {
      static  string baseurl="https://192.168.10.27:443/data/";
     static  string slog="20170629172";
  static string skey="ZDL02001201706";
  static string publickey="MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDTleXSqOajvEwt8RhDYNZd4guh"+
"DiGWgeFiSt4c1eFigalhaGB6+KL6PSV3wEMgbe2x1fKJoS52Qi7Vxu4w64TS5xmB"+
"DUzcndO9FhjGYO1CoHIyO9AtczzPBePDYcd2tk+gjMSpf+Z3jMnTGVgRDBkSpqi9"+
"YbnUmtfA8JpVJEviMwIDAQAB";
static string clientPrivateKey="MIICXAIBAAKBgQDTleXSqOajvEwt8RhDYNZd4guhDiGWgeFiSt4c1eFigalhaGB6"+
"+KL6PSV3wEMgbe2x1fKJoS52Qi7Vxu4w64TS5xmBDUzcndO9FhjGYO1CoHIyO9At"+
"czzPBePDYcd2tk+gjMSpf+Z3jMnTGVgRDBkSpqi9YbnUmtfA8JpVJEviMwIDAQAB"+
"AoGAVwib5tGPPd7gvy0jO+QDic7H1dIIQu7eFR6Syu23rluDnwveU/ceoyyv0tiF"+
"RDuzwKkvASoKAJ8swMb5h6n5kkABOQzMNz2aozisssFA7QoA5QChdLFyaHsKmfTo"+
"mLOXVvsQKutQFWA2a/2wjMgtuy7cM/TU2WL1pbh4pUDHYeECQQDpKwIqTiyBXJKE"+
"/QF+hmjdNMB0qsFYensxhiLYql+Y9bFO7tXdWTBdI4ZK3SW5W8RYFUCXI8jXa4PS"+
"SQynTWU7AkEA6E3dsnT97No5Lbiwym+t60u8et4wFGLaUhrXB54Sda9gW2ooiVH+"+
"9G7SK79Jq8pcG984ydGauMb1oSdGO85HaQJAVtw8vEHO9onj00LlMZskqXMjVtLd"+
"n/ZQukw74vblEfhFCyCR7xlwmOHI/06O5RQ4eo/ANg2Qnh9hRg8Mda6xTQJAcC4q"+
"ASO9+8LmGc42kYuc0SOhwTPKxA14oG2VqXgMMgie34ZETQvrst5RYA7f5LW0BUGm"+
"SR/UdCipl0fY1mR5ecqYe+BdNRg2I/pHx3L/y7aIYek=";
        static void Main(string[] args)
        {    

           var ret= domicile("","");
            Console.WriteLine("Hello World!"+ret);
        }
 
           private static string domicile(string api_id, string api_secret)
        {
        //      params.put("time", time_now);
        // params.put("skey", skey);
        // params.put("slog", slog);
    //   String slog = params.get("slog").toString();
    //     params.remove("slog");
    //     String sign = EncryUtil.handleRSA(params, clientPrivateKey);
    //     params.put("sign", sign);
    //     String info = JSON.toJSONString(params);
    //     String aesKey = RandomUtil.getRandom(16);
    //     String data = AES.encryptToBase64(info, aesKey);
    //     String encryptkey = RSA.encrypt(aesKey, serverPublicKey);
var bs=Convert.FromBase64String(string.Format(""));
   // var  rsa=RSA.Create();rsa.SignHash
   //   var sbs=  rsa.Encrypt(bs,clientPrivateKey);
            using (var client = new HttpClient()){
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            using (var formData = new MultipartContent())
            {
                HttpContent hslog = new StringContent("data=12201607001&encryptkey=20170629172&logid=20170629172", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = client.PostAsync(
                    baseurl+"getDrivingLicenseBySfzmhm",
                   hslog ).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.Write(response.ToString());
                    return string.Empty;
                }
                return response.Content.ReadAsStringAsync().Result;
            }}
        }
    }
}
