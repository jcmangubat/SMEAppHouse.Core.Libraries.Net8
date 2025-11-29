using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SMEAppHouse.Core.ScraperBox.Selenium
{
    public static class Helper
    {
        /// <summary>
        /// Grabs the page content from the specified URL using ChromeDriver in headless mode.
        /// </summary>
        /// <param name="targetPgUrl">The target page URL to load</param>
        /// <param name="proxy">Optional proxy tuple (host, port) for the request</param>
        /// <returns>The page source content as a string</returns>
        public static string GrabPage(string targetPgUrl, Tuple<string, string>? proxy = null)
        {
            IWebDriver? driver = null;
            Exception? exception = null;
            var content = string.Empty;

            try
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--disable-gpu");
                chromeOptions.AddArgument("--no-sandbox");
                chromeOptions.AddArgument("--disable-dev-shm-usage");
                
                if (proxy != null)
                {
                    var proxyObj = new Proxy { HttpProxy = $"{proxy.Item1}:{proxy.Item2}" };
                    chromeOptions.Proxy = proxyObj;
                }
                
                driver = new ChromeDriver(chromeOptions);
                driver.Navigate().GoToUrl(targetPgUrl);
                content = driver.PageSource;
                return content;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (driver != null)
                {
                    Task.Delay(2000).ContinueWith( t =>
                    {
                        Thread.Sleep(1000);
                        driver.Quit();
                        driver.Dispose();
                    });
                }

                if (exception != null)
                    throw exception;
            }

            return content;
        }

        /// <summary>
        /// Grabs the page content using an existing WebDriver instance.
        /// </summary>
        /// <param name="driver">The WebDriver instance to use</param>
        /// <param name="targetPgUrl">The target page URL to load</param>
        /// <param name="proxy">Optional proxy tuple (host, port) - Note: Proxy must be configured when creating the driver</param>
        /// <returns>The page source content as a string</returns>
        public static string? GrabPage(IWebDriver driver, string targetPgUrl, Tuple<string, string>? proxy = null)
        {
            if (driver == null)
                throw new InvalidOperationException("driver was not supplied");

            // Note: Proxy configuration should be done when creating the ChromeDriver instance
            // This method accepts a proxy parameter for API compatibility but doesn't apply it here
            // as proxy settings need to be set via ChromeOptions when creating the driver

            driver.Navigate().GoToUrl(targetPgUrl);
            return driver?.PageSource;
        }

    }
}
