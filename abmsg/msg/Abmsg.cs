using System;
using System.Collections.Generic;

namespace abmsg.msg
{
    public partial class Abmsg
    {
        public string Ordinal { get; set; }
        public bool Busiflag { get; set; }
        public short Busitype { get; set; }
        public string Content { get; set; }
        public int Count { get; set; }
        public string Identity { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool Sendflag { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
