using PuppeteerSharp;
using SMEAppHouse.ScraperBox.Common.Interfaces;

namespace SMEAppHouse.ScraperBox.Puppeteer.Interface
{
    public interface ICrawlerResultPuppeteer : ICrawlerResult
    {
        string Url { get; set; }
        Page HtmlPage { get; set; }
    }
}