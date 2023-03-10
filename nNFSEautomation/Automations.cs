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
        private static string _user { get; set; }
        private static string _password { get; set; }
        public static string DayOfDateRequest { get; set; }
        public static string MonthOfDateRequest { get; set; }
        public static string YearOfDateRequest { get; set; }
        private static string _xmlString { get; set; }
        private static string _nfeString { get; set; }
        private static string _dateOfRequest { get; set; }
        public static IWebDriver Driver => WebDriver.Driver;
        public static WebDriverWait Wait => new(Driver, TimeSpan.FromMinutes(5));
        private static ConnectToDB _dataBase = new();


        public void GoToWebSite(string url)
        {
            Driver.Navigate().GoToUrl(url);
            Driver.Manage().Window.Maximize();
            _dataBase.CreateLoginsCollection();
            _dataBase.CreateClientsCollection();
            _dataBase.CreateXmlCollection();
        }

        public void Login(string user, string password)
        {
            IWebElement inputUser = HelperPath.ValidationToReturnElement(PathsToAutomations.ByUserInput, 1, 5);
            inputUser.Clear();
            inputUser.SendKeys(user);
            _user = user;

            IWebElement inputPassword = HelperPath.ValidationToReturnElement(PathsToAutomations.ByPasswordInput, 1, 5);
            inputPassword.Clear();
            inputPassword.SendKeys(password);
            _password = password;

            HelperPath.ValidationToReturnElement(PathsToAutomations.ByBtnLogin, 1, 5).Click();

            try
            {
                if(Driver.FindElement(By.XPath(PathsToAutomations.ByBtnMenu)).Displayed)
                {
                    XmlLogin xmlLogin = new();
                    xmlLogin.User = _user;
                    xmlLogin.Password = _password;
                    _dataBase.InsertLoginInDataBase(xmlLogin);

                    Console.WriteLine("Escolha a data em que as requisiçõe serão feitas: ");
                    HelperPath.ValidationToReturnElement(PathsToAutomations.BySelectMax, 1, 5).Click();

                    _dateOfRequest = SetDate();
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
                XmlClient xmlClient = new();
                xmlClient.CpfCnpjClient = Driver.FindElement(By.XPath($"//*[@id=\"frmDados:j_idt91:dtResultado_data\"]/tr[{index + 1}]/td[2]")).Text;
                xmlClient.NameClient = Driver.FindElement(By.XPath($"//*[@id=\"frmDados:j_idt91:dtResultado_data\"]/tr[{index + 1}]/td[3]")).Text;
                _dataBase.InsertClientsInDataBase(xmlClient);

                Driver.FindElement(By.XPath($"//*[@id=\"frmDados:j_idt91:dtResultado:{index}:j_idt95\"]/i")).Click();
                Wait.Until(x => x.FindElement(By.XPath(PathsToAutomations.ByWebcomeHeader)).Enabled);
                NavigateToNfeConsult();
                HelperPath.ValidationToReturnElement(PathsToAutomations.BySelectMax, 1, 5).Click();
            }
        }

        public static void NavigateToNfeConsult()
            {
            HelperPath.ValidationToReturnElement(PathsToAutomations.ByBtnGerenciarNfe, 1, 5).Click();
            Wait.Until(x => x.FindElement(By.XPath(PathsToAutomations.ByBtnExportarNota)).Enabled);
            HelperPath.ValidationToReturnElement(PathsToAutomations.ByBtnExportarNota, 1, 5).Click();
            try
            {
                HelperPath.ValidationToReturnElement(PathsToAutomations.ByInputDate, 1, 5).Clear();
                HelperPath.ValidationToReturnElement(PathsToAutomations.ByInputDate, 1, 5).SendKeys(_dateOfRequest);
                HelperPath.ValidationToReturnElement(PathsToAutomations.BySelectXmlRange, 1, 5).Clear();
                HelperPath.ValidationToReturnElement(PathsToAutomations.BySelectXmlRange, 1, 5).SendKeys("100");
                HelperPath.ValidationToReturnElement(PathsToAutomations.ByPesquisar, 1, 5).Click();

                Wait.Until(x => HelperPath.ValidationToReturnElement(PathsToAutomations.ByProcessando, 1, 5).GetAttribute("class") == "swal-overlay");

                if (HelperPath.ValidationToReturnElement(PathsToAutomations.ByVerifyXmls, 1, 5).Text == "Nenhum registro encontrado")
                {
                    Driver.FindElement(By.XPath(PathsToAutomations.BySelectClients)).Click();
                }
                else
                {
                    _xmlString = PostXmlRequest();
                    FilterXml(_xmlString);
                    Driver.FindElement(By.XPath(PathsToAutomations.BySelectClients)).Click();
                }
            }catch (Exception ex)
            {
                Driver.FindElement(By.XPath(PathsToAutomations.BySelectClients)).Click();
                return;
            }
        }

        public static string SetDate()
        {
            do
            {
                try
                {
                    Console.WriteLine("Digite o dia para buscar os xmls: ");
                    DayOfDateRequest = Console.ReadLine();
                    Console.WriteLine("Digite o nome do mês: ");
                    MonthOfDateRequest = Console.ReadLine().ToLower();
                    Console.WriteLine("Digite o ano: ");
                    YearOfDateRequest = Console.ReadLine();

                    FormatDate format = new();
                    format.FormatDay(DayOfDateRequest);
                    format.FormatMonth(MonthOfDateRequest);
                    format.FormatYear(YearOfDateRequest);

                    return $"{DayOfDateRequest}{MonthOfDateRequest}{YearOfDateRequest}";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            } while (true);
        }

        public static string PostXmlRequest()
        {
            CookieContainer cookieContainer = AutomationCookies.GetCookies();

            string xmlForm = AutomationForms.GetForms();

            byte[] xmlFormBytes = Encoding.ASCII.GetBytes(xmlForm);

            HttpWebRequest requestWeb = (HttpWebRequest)HttpWebRequest.Create("https://notajoseense.sjc.sp.gov.br/notafiscal/paginas/exportacaonota/exportacaoNota.jsf");
            requestWeb.Method = "POST";
            requestWeb.Host = "notajoseense.sjc.sp.gov.br";
            requestWeb.KeepAlive = true;
            requestWeb.ContentLength = xmlFormBytes.LongLength;
            requestWeb.Headers.Add("Cache-Control", "max-age=0");
            requestWeb.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"111\", \"Not(A:Brand\";v=\"8\", \"Chromium\";v=\"111\"");
            requestWeb.Headers.Add("sec-ch-ua-mobile", "?0");
            requestWeb.Headers.Add("sec-ch-ua-platform", "\"Windows\"");
            requestWeb.Headers.Add("Upgrade-Insecure-Requests", "1");
            requestWeb.Headers.Add("Origin", "https://notajoseense.sjc.sp.gov.br");
            requestWeb.ContentType = "application/x-www-form-urlencoded";
            requestWeb.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36");
            requestWeb.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            requestWeb.Headers.Add("Sec-Fetch-Site", "same-origin");
            requestWeb.Headers.Add("Sec-Fetch-Mode", "navigate");
            requestWeb.Headers.Add("Sec-Fetch-User", "?1");
            requestWeb.Headers.Add("Sec-Fetch-Dest", "document");
            requestWeb.Referer = "https://notajoseense.sjc.sp.gov.br/notafiscal/paginas/exportacaonota/exportacaoNota.jsf";
            requestWeb.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            requestWeb.Headers.Add("Accept-Language", "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            requestWeb.ProtocolVersion = new Version(1, 1);
            requestWeb.CookieContainer = cookieContainer;

            using (var stream = requestWeb.GetRequestStream())
            {
                stream.Write(xmlFormBytes, 0, xmlFormBytes.Length);
            }

            WebResponse response = requestWeb.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        public static void FilterXml(string xml)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(xml);

            XmlNamespaceManager nsm = new(xmlDoc.NameTable);
            nsm.AddNamespace("xmlF", "http:/www.abrasf.org.br/nfse.xsd");

            XmlNodeList nfelote = xmlDoc.SelectNodes(NodePaths.NfeInXml, nsm);

            ReadLote(nfelote);
        }

        public static void ReadLote(XmlNodeList lote)
        {
            for(int index = 0; index < lote.Count; index++)
            {
                string nfe = lote[index].OuterXml;
                _nfeString = nfe;
                _dataBase.InsertXmlInDataBase(FilterNfe(_nfeString));
            }
        }

        public static XmlInfos FilterNfe(string nfe)
        {
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(nfe);

            XmlNamespaceManager nsm = new(xmlDoc.NameTable);
            nsm.AddNamespace("nfeF", "http:/www.abrasf.org.br/nfse.xsd");

            XmlNode nodeNumero = xmlDoc.SelectSingleNode(NodePaths.Numero, nsm);
            string numero = nodeNumero.InnerText;

            XmlNode nodeCodigoVerificacao = xmlDoc.SelectSingleNode(NodePaths.CodigoVerificacao, nsm);
            string codigoVerificacao = nodeCodigoVerificacao.InnerText;

            XmlNode nodeDataEmissao = xmlDoc.SelectSingleNode(NodePaths.DataEmissao, nsm);
            string dataEmissao = nodeDataEmissao.InnerText;

            XmlNode nodeValorServico = xmlDoc.SelectSingleNode(NodePaths.ValorServico, nsm);
            string valorServico = nodeValorServico.InnerText;

            XmlNode nodeCnpjPrestador = xmlDoc.SelectSingleNode(NodePaths.CnpjPrestador, nsm);
            string cnpjPrestador = nodeCnpjPrestador.InnerText;

            XmlNode nodeRazaoSocialPrestador = xmlDoc.SelectSingleNode(NodePaths.RazaoSocialPrestador, nsm);
            string razaoSocialPrestador = nodeRazaoSocialPrestador.InnerText;

            string cpfcnpjTomador = string.Empty;
            try
            {
                XmlNode nodeCnpjTomador = xmlDoc.SelectSingleNode(NodePaths.CnpjTomador, nsm);
                cpfcnpjTomador = nodeCnpjTomador.InnerText;
            }
            catch (Exception)
            {
                XmlNode nodeCpfTomador = xmlDoc.SelectSingleNode(NodePaths.CpfTomador, nsm);
                cpfcnpjTomador = nodeCpfTomador.InnerText;
            }

            XmlNode nodeRazaoSocialTomador = xmlDoc.SelectSingleNode(NodePaths.RazaoSocialTomador, nsm);
            string razaoSocialTomador = nodeRazaoSocialTomador.InnerText;

            XmlInfos xmlInfos = new XmlInfos(numero, codigoVerificacao, dataEmissao, valorServico, cnpjPrestador, razaoSocialPrestador, cpfcnpjTomador, razaoSocialTomador);

            return xmlInfos;
        }

        public void Dispose()
        {
            Driver.Dispose();
            Driver.Quit();
        }
    }
}