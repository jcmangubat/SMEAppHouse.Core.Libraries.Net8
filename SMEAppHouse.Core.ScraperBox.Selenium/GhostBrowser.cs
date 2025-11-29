using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SMEAppHouse.Core.ScraperBox.Models;

namespace SMEAppHouse.Core.ScraperBox.Selenium
{
    /// <summary>
    /// Headless browser wrapper using ChromeDriver for web scraping operations.
    /// </summary>
    public class GhostBrowser : IDisposable
    {
        public IWebDriver WebDriver { get; set; }

        /// <summary>
        /// Initializes a new instance of GhostBrowser with ChromeDriver in headless mode.
        /// </summary>
        /// <param name="proxy">Optional proxy configuration for the browser</param>
        public GhostBrowser(IPProxy? proxy = null)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            
            if (proxy != null)
            {
                var proxyObj = new Proxy 
                { 
                    HttpProxy = $"{proxy.IPAddress}:{proxy.PortNo}",
                    SslProxy = $"{proxy.IPAddress}:{proxy.PortNo}"
                };
                chromeOptions.Proxy = proxyObj;
            }
            
            this.WebDriver = new ChromeDriver(chromeOptions);
        }

        /// <summary>
        /// Updates the proxy configuration for the browser.
        /// Note: ChromeDriver requires proxy to be set during initialization.
        /// This method will recreate the driver with the new proxy settings.
        /// </summary>
        /// <param name="proxy">The proxy configuration to use</param>
        public void UpdateProxy(IPProxy proxy)
        {
            // Dispose existing driver
            if (this.WebDriver != null)
            {
                try
                {
                    this.WebDriver.Quit();
                    this.WebDriver.Dispose();
                }
                catch
                {
                    // Ignore disposal errors
                }
            }

            // Create new driver with updated proxy
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            
            if (proxy != null)
            {
                var proxyObj = new Proxy 
                { 
                    HttpProxy = $"{proxy.IPAddress}:{proxy.PortNo}",
                    SslProxy = $"{proxy.IPAddress}:{proxy.PortNo}"
                };
                chromeOptions.Proxy = proxyObj;
            }
            
            this.WebDriver = new ChromeDriver(chromeOptions);
        }

        public void Dispose()
        {
            if (this.WebDriver != null)
            {
                try
                {
                    Task.Delay(1000).ContinueWith(t =>
                    {
                        Thread.Sleep(1000);
                        this.WebDriver.Quit();
                        this.WebDriver.Dispose();
                    });
                }
                catch
                {
                    // Ignore disposal errors
                }
            }
        }
    }
}
