using Knyaz.Optimus;
using OpenQA.Selenium.PhantomJS;
using Console = System.Console;

namespace SMEAppHouse.Core.ScraperBox.OptimusTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var targetPgUrl = "https://www.betfair.com/sport/horse-racing";

            var engine = new Engine();
            var pg = engine.OpenUrl(targetPgUrl);//.Wait();
            pg.Wait();
            var content = pg.Result.Document.Body.InnerHTML;

            var service = PhantomJSDriverService.CreateDefaultService(".\\");
            service.HideCommandPromptWindow = true;

            var driver = new PhantomJSDriver(service);
            content = SMEAppHouse.Core.ScraperBox.Selenium.Helper.GrabPage(driver, targetPgUrl);

            driver.Quit();
        }
    }
}
