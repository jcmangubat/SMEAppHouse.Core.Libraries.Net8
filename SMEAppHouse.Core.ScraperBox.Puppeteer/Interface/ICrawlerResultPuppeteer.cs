using PuppeteerSharp;
using SMEAppHouse.Core.ScraperBox.Interfaces;

namespace SMEAppHouse.Core.ScraperBox.Puppeteer.Interface
{
    public interface ICrawlerResultPuppeteer : ICrawlerResult
    {
        string Url { get; set; }
        Page HtmlPage { get; set; }
    }
}