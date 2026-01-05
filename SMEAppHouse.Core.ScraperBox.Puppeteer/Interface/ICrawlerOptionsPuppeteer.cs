using SMEAppHouse.Core.ScraperBox.Interfaces;

namespace SMEAppHouse.Core.ScraperBox.Puppeteer.Interface
{
    public interface ICrawlerOptionsPuppeteer : ICrawlerOptions
    {
        bool IsHeadless { get; set; }
        bool NoSandbox { get; set; }
        bool No2DCanvas { get; set; }
        bool NoGPU { get; set; }
    }
}