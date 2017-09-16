// {
//  "message":"成功",
//  "status":"1",
//  “code”:”200”,
//  "result":[{
// "DABH":" 370602266232",
// "JZQX":"1",
// "LJJF":"1",
// "RYZT":"1",
// "SFZMHM":"370685198710120552",
// "SYYXQZ":"1",
// "XM":"王明",
// "ZJCX":"C1",
// "ZT":"A"}]
// }
using System.Collections.Generic;

namespace mvc104.models
{
    public class subret
    {
        public string DABH { get; set; }
        public string JZQX { get; set; }
        public string LJJF { get; set; }
         public string RYZT { get; set; }
        public string SFZMHM { get; set; }
        public string SYYXQZ { get; set; }
         public string XM { get; set; }
        public string ZJCX { get; set; }
        public string ZT { get; set; }
    }

 public class getDrivingLicenseBySfzmhm
    {
        public string message { get; set; }
        public string status { get; set; }
        public string code { get; set; }
          public List <subret> result { get; set; }
    }
}