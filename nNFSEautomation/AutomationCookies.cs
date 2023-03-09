using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class AutomationCookies
    {
        public static CookieContainer GetCookies()
        {
            var cookiesFind = WebDriver.Driver.Manage().Cookies.AllCookies;
            List<OpenQA.Selenium.Cookie> cookies = new List<OpenQA.Selenium.Cookie>();
            foreach (OpenQA.Selenium.Cookie cookie in cookiesFind)
            {
                cookies.Add(cookie);
            }

            List<System.Net.Cookie> newCookies = new List<System.Net.Cookie>(cookiesFind.Count);

            for (int count = 0; count < cookies.Count; count++)
            {
                Cookie cookie = new Cookie();
                newCookies.Add(cookie);
                newCookies[count].Name = cookies[count].Name;
                newCookies[count].Value = cookies[count].Value;
                newCookies[count].Path = cookies[count].Path;
                newCookies[count].Domain = cookies[count].Domain;
            }

            CookieContainer cookieContainer = new CookieContainer();
            foreach (var cookie in newCookies)
            {
                cookieContainer.Add(cookie);
            }
            return cookieContainer;
        }
    }
}