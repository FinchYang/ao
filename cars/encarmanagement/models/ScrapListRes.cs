using System.Collections.Generic;

namespace mvc104.models
{
    public class ScrapListRes : commonresponse
    {
        public int count { get; set; }
        public List<Scrap> scraps { get; set; }
    }
}