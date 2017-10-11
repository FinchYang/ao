using System;

namespace mvc104.models
{
    public class tokenticket
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public DateTime last { get; set; }

        public string ticket { get; set; }

        public int ticketexpires_in { get; set; }
        public DateTime ticketlast { get; set; }
    }
}