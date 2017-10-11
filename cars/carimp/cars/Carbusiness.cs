using System;
using System.Collections.Generic;

namespace carimp.cars
{
    public partial class Carbusiness
    {
        public string Identity { get; set; }
        public short Businesstype { get; set; }
        public bool Abroadorservice { get; set; }
        public string Acceptingplace { get; set; }
        public short Cartype { get; set; }
        public string City { get; set; }
        public bool Completed { get; set; }
        public string County { get; set; }
        public DateTime Exporttime { get; set; }
        public DateTime Finishtime { get; set; }
        public bool? Integrated { get; set; }
        public DateTime Losttime { get; set; }
        public string Platenumber1 { get; set; }
        public string Platenumber2 { get; set; }
        public short Platetype { get; set; }
        public string Postaddr { get; set; }
        public DateTime Processtime { get; set; }
        public string Province { get; set; }
        public string QuasiDrivingLicense { get; set; }
        public string Reason { get; set; }
        public short Scrapplace { get; set; }
        public short Status { get; set; }
        public DateTime Time { get; set; }
        public DateTime Waittime { get; set; }
    }
}
