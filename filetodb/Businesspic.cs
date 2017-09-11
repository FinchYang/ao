using System;
using System.Collections.Generic;

namespace importdata
{
    public partial class Businesspic
    {
        public string Identity { get; set; }
        public short Businesstype { get; set; }
        public short Pictype { get; set; }
        public DateTime Time { get; set; }
        public bool Uploaded { get; set; }
    }
}
