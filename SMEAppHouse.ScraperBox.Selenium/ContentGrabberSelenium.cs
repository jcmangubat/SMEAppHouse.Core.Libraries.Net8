using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SMEAppHouse.ScraperBox.Common;
using SMEAppHouse.ScraperBox.Common.Base;
using SMEAppHouse.ScraperBox.Common.Models;
#pragma warning disable 618

namespace SMEAppHouse.ScraperBox.Selenium
{
    public class ContentGrabberSelenium : ContentGrabberBase<string, string>
    {
        public IWebDriver WebDriver { get; set; }

        public ContentGrabberSelenium()
        {
            var service = PhantomJSDriverService.CreateDefaultService(".\\");
            service.HideCommandPromptWindow = true;

            this.WebDriver = new PhantomJSDriver(service);
        }

        public void UpdateProxy(IPProxy proxy)
        {
            if (proxy == null)
                return;

            var script = $"return phantom.setProxy(\"{proxy.IPAddress}\", {proxy.PortNo}, \"http\", \"\", \"";
            _ = ((PhantomJSDriver)this.WebDriver )?.ExecutePhantomJS(script);
        }

        public override bool TryGrabPage(string url, out Task<string> responseTsk, bool inNewPage = false, bool? noImage=false)
        {
            throw new NotImplementedException();
        }

        public override bool TryGrabPage(string url, out Task<string> responseTsk, out string page, bool inNewPage = false, bool? noImage=false)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            Task.Delay(1000).ContinueWith(t =>
            {
                Thread.Sleep(1000);
                this.WebDriver.Quit();
                this.WebDriver.Dispose();
            });
        }
    }
}