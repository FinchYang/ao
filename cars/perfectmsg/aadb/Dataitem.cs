using System;
using System.Collections.Generic;

namespace perfectmsg.aadb
{
    public partial class Dataitem
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public string Defaultvalue { get; set; }
        public short Deleted { get; set; }
        public short Hassecond { get; set; }
        public int Index { get; set; }
        public short Inputtype { get; set; }
        public short Mandated { get; set; }
        public string Name { get; set; }
        public string Seconditem { get; set; }
        public string Statisticstype { get; set; }
        public string Tabletype { get; set; }
        public DateTime Time { get; set; }
        public string Units { get; set; }
    }
}
