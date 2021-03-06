﻿using System;
using System.Collections.Generic;

namespace exportdb
{
    public partial class Business
    {
        public string Identity { get; set; }
        public short Businesstype { get; set; }
        public string Acceptingplace { get; set; }
        public bool Completed { get; set; }
        public DateTime Exporttime { get; set; }
        public DateTime Finishtime { get; set; }
        public bool? Integrated { get; set; }
        public DateTime Losttime { get; set; }
        public string Postaddr { get; set; }
        public DateTime Processtime { get; set; }
        public string QuasiDrivingLicense { get; set; }
        public string Reason { get; set; }
        public short Status { get; set; }
        public DateTime Time { get; set; }
        public DateTime Waittime { get; set; }
    }
}
