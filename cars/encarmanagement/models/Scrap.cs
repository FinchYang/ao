using System;

namespace mvc104.models
{
    public class Scrap
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string id { get; set; }
        public CarType cartype { get; set; }
        public string address { get; set; }
        public DateTime time { get; set; }
    }
}