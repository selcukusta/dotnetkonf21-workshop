using System;
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
    public class BasicTestController : ControllerBase
    {
        private readonly ILogger<BasicTestController> _logger;
        private readonly IWebDriver _driver;

        public BasicTestController(ILogger<BasicTestController> logger)
        {
            _logger = logger;
            var driverOptions = new ChromeOptions();
            //VNC disabled ise aşağıdaki argümanı açabilirsiniz!
            // driverOptions.AddArguments("headless");
            //Namespace -> selenium
            //Service Name -> selenium-hub
            var remoteUri = new Uri("http://selenium-hub.selenium.svc.cluster.local:4444/wd/hub");
            _driver = new RemoteWebDriver(remoteUri, driverOptions);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Payload payload)
        {
            if (payload?.Metadata == null)
            {
                return BadRequest();
            }

            IWebElement header;
            //http://dotnetkonf-ui.default.svc.cluster.local:80
            var testUri = $"http://{payload.Name}-canary.{payload.Namespace}.svc.cluster.local:{payload.Metadata.Port}";
            _logger.LogWarning(testUri);
            _driver.Navigate().GoToUrl(testUri);
            try
            {
                var by = By.ClassName("display-4");
                header = new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until(d => d.FindElement(by));
            }
            catch (NoSuchElementException)
            {
                return StatusCode(500, "Welcome message not found!");
            }

            var content = header.GetAttribute("innerText");
            _logger.LogWarning(content);
            StatusCodeResult result = content.Equals("Welcome") ? Ok() : StatusCode(500);
            _driver.Quit();
            return result;
        }
    }
}
