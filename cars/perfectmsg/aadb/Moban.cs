using System;
using System.Collections.Generic;

namespace perfectmsg.aadb
{
    public partial class Moban
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public short Deleted { get; set; }
        public string Filename { get; set; }
        public string Tabletype { get; set; }
        public DateTime Time { get; set; }
    }
}
