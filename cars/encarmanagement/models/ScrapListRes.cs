using System.Collections.Generic;

namespace mvc104.models
{
    public class ScrapListRes : commonresponse
    {
        public List<Scrap> scraps { get; set; }
    }
}