using System;
using System.Collections.Generic;

namespace perfectmsg.dbmodel
{
    public partial class Summarized
    {
        public string Date { get; set; }
        public string Comment { get; set; }
        public string Content { get; set; }
        public short Draft { get; set; }
        public DateTime Time { get; set; }
    }
}
