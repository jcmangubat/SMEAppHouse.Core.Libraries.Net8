using SMEAppHouse.Core.ScraperBox.Interfaces;

namespace SMEAppHouse.Core.ScraperBox.Models
{
    public class CrawlerOptions : ICrawlerOptions
    {
        public bool InNewPage { get; set; }
        public bool NoImage { get; set; }
        public IPProxy IPProxy { get; set; }
        public bool UseProxy { get; set; }
    }
}

