using System;
using System.Collections.Generic;

namespace importdata
{
    public partial class Businesshis
    {
        public int Ordinal { get; set; }
        public string Acceptingplace { get; set; }
        public short Businesstype { get; set; }
        public bool Completed { get; set; }
        public string Identity { get; set; }
        public string Postaddr { get; set; }
        public string QuasiDrivingLicense { get; set; }
        public DateTime Time { get; set; }
    }
}
