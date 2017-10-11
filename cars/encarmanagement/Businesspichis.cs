using System;
using System.Collections.Generic;

namespace enabo
{
    public partial class Businesspichis
    {
        public int Ordinal { get; set; }
        public short Businesstype { get; set; }
        public string Identity { get; set; }
        public short Pictype { get; set; }
        public DateTime Time { get; set; }
        public bool Uploaded { get; set; }
    }
}
