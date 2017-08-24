namespace mvc104.models
{
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
    public class declarationsignrequest
    {
        public string sign_pic  { get; set; }
    }
    public class uploadpicrequest
    {
        public picType picType { get; set; }

        public string picture { get; set; }
    }
     public class updateinforequest
    {
         public string postaddr { get; set; }
         public bool lost { get; set; }

    }
}