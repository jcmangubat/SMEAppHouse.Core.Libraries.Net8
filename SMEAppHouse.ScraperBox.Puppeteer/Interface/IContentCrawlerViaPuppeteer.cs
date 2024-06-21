using PuppeteerSharp;
using SMEAppHouse.ScraperBox.Common.Interfaces;

namespace SMEAppHouse.ScraperBox.Puppeteer.Interface
{
    public interface IContentCrawlerViaPuppeteer<TCrawlerOptions, TCrawlerResult> : IContentCrawler<TCrawlerOptions, TCrawlerResult>
        where TCrawlerOptions : ICrawlerOptionsPuppeteer
        where TCrawlerResult : ICrawlerResultPuppeteer
    {
        Browser Browser { get; set; }
    }
}