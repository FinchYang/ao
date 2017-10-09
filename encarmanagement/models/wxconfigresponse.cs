namespace mvc104.models
{
    public class wxconfigresponse : commonresponse
    {

        public long timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }
}