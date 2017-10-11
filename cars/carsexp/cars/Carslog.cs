using System;
using System.Collections.Generic;

namespace carsexp.cars
{
    public partial class Carslog
    {
        public int Ordinal { get; set; }
        public short Businesstype { get; set; }
        public string Content { get; set; }
        public string Ip { get; set; }
        public string Method { get; set; }
        public DateTime Time { get; set; }
    }
}
