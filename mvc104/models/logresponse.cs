namespace mvc104.models
{
    public class loginresponse
    {
        public responseStatus status { get; set; }
        public string token { get; set; }
        public short[] okpic{ get; set; }
    }
      public class livingbodyrequest
    {
        public string api_id { get; set; }
         public string api_secret { get; set; }
          public byte[] file { get; set; }
    }
}