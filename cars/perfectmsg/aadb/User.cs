﻿using System;
using System.Collections.Generic;

namespace perfectmsg.aadb
{
    public partial class User
    {
        public User()
        {
            Userlog = new HashSet<Userlog>();
        }

        public string Id { get; set; }
        public short Disabled { get; set; }
        public short Level { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Token { get; set; }
        public short Unitclass { get; set; }
        public string Unitid { get; set; }

        public virtual ICollection<Userlog> Userlog { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
