using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class HelperPath
    {
        public static IWebElement ValidationToReturnElement(string byElement, double timeSpan, int tries)
        {
            int tryAmout = 0;
            do
            {
                try
                {
                    WebDriver.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeSpan);
                    var webElement = WebDriver.Driver.FindElement(By.XPath(byElement));
                    if (webElement != null)
                    {
                        return webElement;
                    }
                }
                catch (Exception)
                {
                    tryAmout++;
                }
            } while (tryAmout < tries);
            throw new Exception("Não foi possível encontrar o elemento!");
        }
    }
}
