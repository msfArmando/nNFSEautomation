using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace nNFSEautomation
{
    class Program
    {
        public static void Main(string[] args)
        {
            bool ciclo = true;
            bool userValid = false;
            bool passwordValid = false;
            var userLogin = string.Empty;
            var passwordLogin = string.Empty;
            bool cicloDate;

            Automations auto = new();

            auto.GoToWebSite("https://notajoseense.sjc.sp.gov.br/notafiscal/paginas/portal/login.html");
            WebDriver.Driver.FindElement(By.XPath(PathsToAutomations.ByEyePassword)).Click();

            while (ciclo)
            {
                if (userValid != true)
                {
                    Console.WriteLine("Digite seu CPF ou CNPJ: ");
                    userLogin = Console.ReadLine();
                    if (userLogin == "")
                    {
                        Console.WriteLine("O campo não pode ser nulo!");
                        continue;
                    }
                    else if (Regex.IsMatch(userLogin, @"^\d+$") == false)
                    {
                        Console.WriteLine("O campo não aceita letras!");
                        continue;
                    }
                }

                if (passwordValid != true)
                {
                    Console.WriteLine("Digite sua senha: ");
                    passwordLogin = Console.ReadLine();
                    if (passwordLogin == "")
                    {
                        Console.WriteLine("O campo não pode ser nulo!");
                        continue;
                    }
                }

                try
                {
                    auto.Login(userLogin, passwordLogin);
                    ciclo = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Erro ao tentar realizar login. Tente novamente com as credenciais corretas!");
                    ciclo = true;
                }
            }

            auto.SelectClient();

            auto.Dispose();
        }
    }
}