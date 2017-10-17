using System;
using System.Collections.Generic;

namespace encm.msg
{
    public partial class Drivermsg
    {
        public int Ordinal { get; set; }
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
