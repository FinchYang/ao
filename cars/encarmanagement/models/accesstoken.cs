namespace mvc104.models
{
    public class accesstoken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
    }


    public class access_idinfo : commonresponse
    {
        public string Identity { get; set; }
        public string photofile { get; set; }
        public businessType businessType { get; set; }
    }
}