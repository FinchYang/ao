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
        public string postaddr { get; set; } = string.Empty;
        public string acceptingplace { get; set; } = string.Empty;
        public string quasiDrivingLicense { get; set; } = string.Empty;
        public string province { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string county { get; set; } = string.Empty;
    }
    public class postaddrrequest
    {
        public string postaddr { get; set; }
        public string acceptingplace { get; set; }
        public string quasiDrivingLicense { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string county { get; set; }

    }
}