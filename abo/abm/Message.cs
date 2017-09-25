using System;
using System.Collections.Generic;

namespace mvc104.abm
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int? Historyid { get; set; }
        public string Sent { get; set; }
        public DateTime? Time { get; set; }
    }
}
