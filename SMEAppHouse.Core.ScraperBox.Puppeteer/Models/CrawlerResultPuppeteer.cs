using PuppeteerSharp;
using SMEAppHouse.Core.ScraperBox.Models;
using SMEAppHouse.Core.ScraperBox.Puppeteer.Interface;

namespace SMEAppHouse.Core.ScraperBox.Puppeteer.Models
{
    public class CrawlerResultPuppeteer : CrawlerResult, ICrawlerResultPuppeteer
    {
        public Page HtmlPage { get; set; }
        public string Url { get; set; }
    }
}