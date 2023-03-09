using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V108.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cookie = System.Net.Cookie;
using MongoDB.Driver;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using MongoDB.Driver.Core.Operations;
using OpenQA.Selenium.DevTools.V109.Network;
using System.Web;
using System.Collections.Specialized;
using System.Security.Principal;
using MongoDB.Bson.Serialization.Serializers;
using nNFSEautomation;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace nNFSEautomation
{
    public class Automations : IDisposable
    {
        public static string DayOfDateRequest { get; set; }
        public static string MonthOfDateRequest { get; set; }
        public static string YearOfDateRequest { get; set; }
        private static string _xmlString { get; set; }
        public static IWebDriver Driver => WebDriver.Driver;
        public static WebDriverWait Wait => new(Driver, TimeSpan.FromMinutes(5));

        public void GoToWebSite(string url)
        {
            Driver.Navigate().GoToUrl(url);
            Driver.Manage().Window.Maximize();
        }

        public void Login(string user, string password)
        {
            IWebElement inputUser = HelperPath.ValidationToReturnElement(PathsToAutomations.ByUserInput, 1, 5);
            inputUser.Clear();
            inputUser.SendKeys(user);

            IWebElement inputPassword = HelperPath.ValidationToReturnElement(PathsToAutomations.ByPasswordInput, 1, 5);
            inputPassword.Clear();
            inputPassword.SendKeys(password);

            HelperPath.ValidationToReturnElement(PathsToAutomations.ByBtnLogin, 1, 5).Click();

            try
            {
                if(Driver.FindElement(By.XPath(PathsToAutomations.ByBtnMenu)).Displayed)
                {
                    return;
                }
            }
            catch
            {
                throw new Exception("Não foi possível realizar o login!");
            }
        }

        public void SelectClient()
        {
            IWebElement tableClients = HelperPath.ValidationToReturnElement(PathsToAutomations.ByClientsTable, 1, 5);
            var rows = tableClients.FindElements(By.TagName("tr"));
            for(int index = 0; index < rows.Count; index++)
            {
                Driver.FindElement(By.XPath($"//*[@id=\"frmDados:j_idt91:dtResultado:{index}:j_idt95\"]/i")).Click();
                Wait.Until(x => x.FindElement(By.XPath(PathsToAutomations.ByWebcomeHeader)).Enabled);
                NavigateToNfeConsult();
            }
        }

        public static void NavigateToNfeConsult()
            {
            HelperPath.ValidationToReturnElement(PathsToAutomations.ByBtnGerenciarNfe, 1, 5).Click();
            Wait.Until(x => x.FindElement(By.XPath(PathsToAutomations.ByBtnExportarNota)).Enabled);
            HelperPath.ValidationToReturnElement(PathsToAutomations.ByBtnExportarNota, 1, 5).Click();

            HelperPath.ValidationToReturnElement(PathsToAutomations.ByInputDate, 1, 5).Clear();
            HelperPath.ValidationToReturnElement(PathsToAutomations.ByInputDate, 1, 5).SendKeys(SetDate());
            HelperPath.ValidationToReturnElement(PathsToAutomations.ByPesquisar, 1, 5).Click();

            Wait.Until(x => HelperPath.ValidationToReturnElement(PathsToAutomations.ByProcessando, 1, 5).GetAttribute("class") == "swal-overlay");
        }

        public static string SetDate()
        {
            Console.WriteLine("Digite o dia para buscar os xmls: ");
            string searchDay = Console.ReadLine();
            Console.WriteLine("Digite o nome do mês: ");
            string searchMonth = Console.ReadLine().ToLower();
            searchMonth = $"{searchMonth.Substring(0, 3)}";
            Console.WriteLine("Digite o ano: ");
            YearOfDateRequest = Console.ReadLine();

            FormatDate format = new();
            DayOfDateRequest = format.FormatDay(searchDay);
            MonthOfDateRequest = format.FormatMonth(searchMonth);

            return $"{DayOfDateRequest}{MonthOfDateRequest}{YearOfDateRequest}";
        }

        public string PostXmlRequest()
        {
            CookieContainer cookieContainer = AutomationCookies.GetCookies();

            string xmlForm = AutomationForms.GetForms();

            byte[] xmlFormBytes = Encoding.ASCII.GetBytes(xmlForm);

            HttpWebRequest requestWeb = (HttpWebRequest)HttpWebRequest.Create("https://notajoseense.sjc.sp.gov.br/notafiscal/paginas/exportacaonota/exportacaoNota.jsf");
            requestWeb.Method = "POST";
            requestWeb.ProtocolVersion = new Version(1, 1);
            requestWeb.Host = "notajoseense.sjc.sp.gov.br";
            requestWeb.KeepAlive = true;
            requestWeb.ContentLength = xmlFormBytes.LongLength;
            requestWeb.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            requestWeb.Headers.Add("sec-ch-ua", "\"Chromium\";v=\"110\", \"Not A(Brand\";v=\"24\", \"Google Chrome\";v=\"110\"");
            requestWeb.Headers.Add("sec-ch-ua-mobile", "?0");
            requestWeb.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            requestWeb.Headers.Add("Upgrade-Insecure-Requests", "1");
            requestWeb.Headers.Add("Origin", "https://notajoseense.sjc.sp.gov.br");
            requestWeb.ContentType = "application/x-www-form-urlencoded";
            requestWeb.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
            requestWeb.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            requestWeb.Headers.Add("Sec-Fetch-Site", "same-origin");
            requestWeb.Headers.Add("Sec-Fetch-Mode", "navigate");
            requestWeb.Headers.Add("Sec-Fetch-User", "?1");
            requestWeb.Headers.Add("Sec-Fetch-Dest", "document");
            requestWeb.Referer = "https://notajoseense.sjc.sp.gov.br/notafiscal/paginas/exportacaonota/exportacaoNota.jsf";
            requestWeb.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            requestWeb.Headers.Add(HttpRequestHeader.AcceptLanguage, "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            requestWeb.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.Deflate;

            using (var stream = requestWeb.GetRequestStream())
            {
                stream.Write(xmlFormBytes, 0, xmlFormBytes.Length);
            }

            WebResponse response = requestWeb.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return _xmlString = reader.ReadToEnd();
            }
        }

        public void Dispose()
        {
            Driver.Dispose();
            Driver.Quit();
        }
    }
}
