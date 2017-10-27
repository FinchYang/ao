using System;
using System.Collections.Generic;

namespace perfectmsg.dbmodel
{
    public partial class Dataitem
    {
        public int Id { get; set; }
        public bool Centerdisplay { get; set; }
        public string Comment { get; set; }
        public short Datatype { get; set; }
        public bool Deleted { get; set; }
        public short Inputtype { get; set; }
        public bool Mandated { get; set; }
        public string Name { get; set; }
        public string Seconditem { get; set; }
        public DateTime Time { get; set; }
        public bool Unitdisplay { get; set; }
    }
}
