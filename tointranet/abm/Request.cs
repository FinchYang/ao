﻿using System;
using System.Collections.Generic;

namespace exportdb.abm
{
    public partial class Request
    {
        public int Ordinal { get; set; }
        public string Content { get; set; }
        public string Ip { get; set; }
        public string Method { get; set; }
        public DateTime Time { get; set; }
    }
}
