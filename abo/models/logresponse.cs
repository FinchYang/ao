using System;

namespace mvc104.models
{
    public class loginresponse : commonresponse
    {
        public string token { get; set; }
        public short[] okpic { get; set; }
        public bool submitted { get; set; }
        public businessstatus businessstatus { get; set; }
        public DateTime wait_time { get; set; }
        public DateTime process_time { get; set; }
        public DateTime finish_time { get; set; }
    }
    public class livingbodyrequest
    {
        public string api_id { get; set; }
        public string api_secret { get; set; }
        public byte[] file { get; set; }
    }
}