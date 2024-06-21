using SMEAppHouse.ScraperBox.Common.Models;

namespace SMEAppHouse.ScraperBox.Common.Interfaces
{
    public interface ICrawlerOptions
    {
        bool InNewPage { get; set; }
        bool NoImage { get; set; }
        IPProxy IPProxy { get; set; }
        bool UseProxy { get; set; }
    }
}