﻿using System;
using System.Collections.Generic;

namespace mvc104
{
    public partial class Business
    {
        public string Identity { get; set; }
        public short Businesstype { get; set; }
        public bool Completed { get; set; }
        public DateTime Time { get; set; }
    }
}
