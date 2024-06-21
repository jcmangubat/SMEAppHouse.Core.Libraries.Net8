using PuppeteerSharp;
using SMEAppHouse.ScraperBox.Common.Models;
using SMEAppHouse.ScraperBox.Puppeteer.Interface;

namespace SMEAppHouse.ScraperBox.Puppeteer.Models
{
    public class CrawlerResultPuppeteer : CrawlerResult, ICrawlerResultPuppeteer
    {
        public Page HtmlPage { get; set; }
        public string Url { get; set; }
    }
}