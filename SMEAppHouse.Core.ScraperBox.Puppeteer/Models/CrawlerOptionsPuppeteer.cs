using SMEAppHouse.Core.ScraperBox.Models;
using SMEAppHouse.Core.ScraperBox.Puppeteer.Interface;

namespace SMEAppHouse.Core.ScraperBox.Puppeteer.Models
{
    public class CrawlerOptionsPuppeteer : CrawlerOptions, ICrawlerOptionsPuppeteer
    {
        public bool IsHeadless { get; set; }
        public bool NoSandbox { get; set; }
        public bool No2DCanvas { get; set; }
        public bool NoGPU { get; set; }

        public CrawlerOptionsPuppeteer()
        {
            IsHeadless = true;
            NoSandbox = true;
            No2DCanvas = true;
            NoGPU = true;
        }
    }
}