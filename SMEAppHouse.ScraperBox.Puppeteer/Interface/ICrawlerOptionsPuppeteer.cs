using SMEAppHouse.ScraperBox.Common.Interfaces;

namespace SMEAppHouse.ScraperBox.Puppeteer.Interface
{
    public interface ICrawlerOptionsPuppeteer : ICrawlerOptions
    {
        bool IsHeadless { get; set; }
        bool NoSandbox { get; set; }
        bool No2DCanvas { get; set; }
        bool NoGPU { get; set; }
    }
}