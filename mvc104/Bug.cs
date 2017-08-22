using System;
using System.Collections.Generic;

namespace mvc104
{
    public partial class Bug
    {
        public int Idbug { get; set; }
        public byte[] Attachment { get; set; }
        public string Handler { get; set; }
        public string Reply { get; set; }
        public string Sketch { get; set; }
        public string Specification { get; set; }
        public string Submitter { get; set; }
    }
}
