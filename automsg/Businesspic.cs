using System;
using System.Collections.Generic;

namespace exportdb
{
    public partial class Businesspic
    {
        public string Identity { get; set; }
        public int Businesstype { get; set; }
        public short Pictype { get; set; }
        public DateTime Time { get; set; }
        public bool Uploaded { get; set; }
    }
}
