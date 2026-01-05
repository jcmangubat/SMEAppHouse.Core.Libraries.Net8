using SMEAppHouse.Core.ScraperBox.Models;

namespace SMEAppHouse.Core.ScraperBox.Interfaces
{
    public interface ICrawlerOptions
    {
        bool InNewPage { get; set; }
        bool NoImage { get; set; }
        IPProxy IPProxy { get; set; }
        bool UseProxy { get; set; }
    }
}

