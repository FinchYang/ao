using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
    {
        public TrustAllCertificatePolicy()
        { }

        public bool CheckValidationResult(ServicePoint sp,
            System.Security.Cryptography.X509Certificates.X509Certificate cert,
            WebRequest req, int problem)
        {
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var dict = new Dictionary<string, string>();
            dict.Add("data", "l++gkFMgAQX/W2XTB2tT6R3+bCC0Br54WyPQXP+OrX0PCk6sQBxqFN5VcAjirQipDRS5WoIa8Uc1nUszCldVB4Wz0TnQm3osNV8AUVel67zBv8qbhKxuKnGCueFymjQp9iKX0QC5mepFuKu+UkVguuXZrWnvG07HOFaUbz/YPiY2h4JOYS1gqk4d1Yrr37BcO+16NtcbKVrdBgsY5Vp4Ya0lNqaV+ypXScNQsQUj1uTQcKMfrkOUAgVE/iogkuIwI1TNXK/hj3FnEpI5PkNMhhS/tXRSxverltCE8anbIpTU/LnvIWpSVqnAqqYHkWSgrfjkNaxWi2/8idG7erwKmIejTrdOYXzzR0BQ9PPqvFQ=");
            dict.Add("encryptkey", "G6SHPdEKU1p8STyFivLEksHSR2wd7UJI6KcG33kWGY+c64PCnMcPrUUQnAxQ+8L+znwklI2dc37/ZuQHo0YvbVezpCXEJrha6skvOBYbqU8Di9JqCoZWdjUq1gApcQw5V9Rnk/pW2JrNvfEBjeOwMH5AgBhvWOWNStoSNk4M+o8=");
            dict.Add("logid", "12201607001");

            using (var client = new HttpClient())
            using (var formData = new FormUrlEncodedContent(dict))
            {
                var ret = client.PostAsync("https://192.168.10.27:443/data/getDrivingLicenseBySfzmhm", formData);

                Console.WriteLine(ret.Result.Content.ReadAsStringAsync().Result);
            }
        }
    }
}

