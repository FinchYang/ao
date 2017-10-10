namespace mvc104.models
{
    public class getaosresponse : commonresponse
    {
        public bool abroadorservice { get; set; }

    }
    public class getlosttimeresponse : commonresponse
    {
        public System.DateTime losttime { get; set; }

    }
    public class getaddrresponse : commonresponse
    {
        public string detailedAddress { get; set; }
        public string plateNumber1 { get; set; }
        public string plateNumber2 { get; set; }
        public ScrapPlace scrapPlace { get; set; }
        public CarType carType { get; set; }
        public PlateType plateType { get; set; }
        public string province { get; set; } 
        public string city { get; set; } 
        public string county { get; set; } 
    }
    public class Postaddrrequest
    {
        public string detailedAddress { get; set; }
        public string acceptingplace { get; set; }
        public string plateNumber1 { get; set; }
        public string plateNumber2 { get; set; }
        public ScrapPlace scrapPlace { get; set; }
        public CarType carType { get; set; }
        public PlateType plateType { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string county { get; set; }

    }
}