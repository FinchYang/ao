using System;
using System.Collections.Generic;

namespace perfectmsg.aadb
{
    public partial class Reportsdata
    {
        public string Date { get; set; }
        public string Unitid { get; set; }
        public string Rname { get; set; }
        public string Comment { get; set; }
        public string Content { get; set; }
        public string Declinereason { get; set; }
        public short Draft { get; set; }
        public short Signtype { get; set; }
        public DateTime Submittime { get; set; }
        public DateTime Time { get; set; }
    }
}
