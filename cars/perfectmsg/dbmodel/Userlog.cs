using System;
using System.Collections.Generic;

namespace perfectmsg.dbmodel
{
    public partial class Userlog
    {
        public int Ordinal { get; set; }
        public string Content { get; set; }
        public string Ip { get; set; }
        public DateTime Time { get; set; }
        public string Userid { get; set; }

        public virtual User User { get; set; }
    }
}
