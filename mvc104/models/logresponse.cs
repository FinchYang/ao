using System;

namespace mvc104.models
{
    public enum responseStatus
    {
        ok, iderror, nameerror, phoneerror, tokenerror,
        requesterror, imageerror, fileprocesserror, access_tokenerror, ticketerror
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
}