using System;

namespace mvc104.models
{
    public enum responseStatus
    {
        ok, iderror, nameerror, phoneerror, tokenerror,
        requesterror, imageerror, fileprocesserror, access_tokenerror, ticketerror
    };
      public enum businessType
    {
        ChangeLicense, delay, lost, damage, overage,
        expire, changeaddr, basicinfo, first, network,three,five
    };
    public class loginresponse
    {
        public responseStatus status { get; set; }
        public string token { get; set; }
    }
    public class commonresponse
    {
        public responseStatus status { get; set; }
    }
    public class wxconfigresponse : commonresponse
    {

        public long timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }
    public class facerequest
    {
        public string image { get; set; }
    }
    public class accesstoken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }
    public class tokenticket
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public DateTime last { get; set; }

        public string ticket { get; set; }

        public int ticketexpires_in { get; set; }
        public DateTime ticketlast { get; set; }
    }
    public class jsticket
    {

        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }

        public int expires_in { get; set; }
    }
       public class changelicenserequest
    {

        public bool lost { get; set; }
        public string postaddr { get; set; }
        public string id_front { get; set; }
         public string id_back { get; set; }
          public string id_inhand { get; set; }
           public string license_pic { get; set; }

 public string sign_pic { get; set; }
 public string hukou_pic { get; set; }


        public int expires_in { get; set; }
    }
}