using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace selenium_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DotnetKonfTestController : ControllerBase
    {
        private readonly ILogger<DotnetKonfTestController> _logger;
        private readonly IWebDriver _driver;

        public DotnetKonfTestController(ILogger<DotnetKonfTestController> logger)
        {
            _logger = logger;
            var driverOptions = new ChromeOptions();
            //VNC disabled ise aşağıdaki argümanı açabilirsiniz!
            // driverOptions.AddArguments("headless");
            //NS -> selenium
            //SN -> selenium-hub
            var remoteUri = new Uri("http://selenium-hub.selenium.svc.cluster.local:4444/wd/hub");
            _driver = new RemoteWebDriver(remoteUri, driverOptions);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        }

        [HttpPost]
        public IActionResult Post()
        {
            IWebElement header;
            var testUri = "http://www.dotnetkonf.com";
            _driver.Navigate().GoToUrl(testUri);
            try
            {
                var by = By.XPath("/html/body/section[3]/div[1]/div[4]/div[2]/h4");
                header = new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until(d => d.FindElement(by));
            }
            catch (NoSuchElementException)
            {
                return StatusCode(500, "Workshop topics area is missing!");
            }

            var content = header.GetAttribute("innerText");
            _logger.LogWarning(content);
            StatusCodeResult result = content.Equals("Workshop Konuları") ? Ok() : StatusCode(500);
            _driver.Quit();
            return result;
        }
    }
}
