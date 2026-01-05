using PuppeteerSharp;
using SMEAppHouse.Core.ScraperBox.Interfaces;

namespace SMEAppHouse.Core.ScraperBox.Puppeteer.Interface
{
    public interface IContentCrawlerViaPuppeteer<TCrawlerOptions, TCrawlerResult> : IContentCrawler<TCrawlerOptions, TCrawlerResult>
        where TCrawlerOptions : ICrawlerOptionsPuppeteer
        where TCrawlerResult : ICrawlerResultPuppeteer
    {
        Browser Browser { get; set; }
    }
}