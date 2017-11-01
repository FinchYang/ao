using System;
using System.Collections.Generic;

namespace perfectmsg.dbmodel
{
    public partial class Unit
    {
        public Unit()
        {
            Reportlog = new HashSet<Reportlog>();
            User = new HashSet<User>();
            Videoreport = new HashSet<Videoreport>();
        }

        public string Id { get; set; }
        public string Ip { get; set; }
        public short Level { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Reportlog> Reportlog { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Videoreport> Videoreport { get; set; }
    }
}
