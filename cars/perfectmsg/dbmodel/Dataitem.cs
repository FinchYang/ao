using System;
using System.Collections.Generic;

namespace perfectmsg.dbmodel
{
    public partial class Dataitem
    {
        public int Id { get; set; }
        public short Centerdisplay { get; set; }
        public string Comment { get; set; }
        public short Datatype { get; set; }
        public short Deleted { get; set; }
        public short Inputtype { get; set; }
        public short Mandated { get; set; }
        public string Name { get; set; }
        public string Seconditem { get; set; }
        public DateTime Time { get; set; }
        public short Unitdisplay { get; set; }
    }
}
