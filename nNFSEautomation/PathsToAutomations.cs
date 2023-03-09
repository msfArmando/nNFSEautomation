using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class PathsToAutomations
    {
        public static string ByUserInput => "//*[@id=\"inputUserName\"]";
        public static string ByPasswordInput => "//*[@id=\"inputPassword\"]";
        public static string ByBtnCaptcha => "//*[@id=\"recaptcha-anchor\"]/div[1]";
        public static string ByBtnLogin => "//*[@id=\"app\"]/div/div/div[2]/div/div[2]/div[2]/form/div[3]/div/button";
        public static string ByEyePassword => "//*[@id=\"app\"]/div/div/div[2]/div/div[2]/div[2]/form/div[2]/div/i";
        public static string ByBtnMenu => "//*[@id=\"header\"]/nav/div[1]";
        public static string ByClientsTable => "//*[@id=\"frmDados:j_idt91:dtResultado_data\"]";
        public static string ByWebcomeHeader => "//*[@id=\"j_idt61\"]/div[2]/div/div/div/div";
        public static string ByBtnGerenciarNfe => "//*[@id=\"13748\"]/a";
        public static string ByBtnExportarNota => "//*[@id=\"13347\"]/a";
        public static string ByInputDate => "//*[@id=\"j_idt62:idDataInicio_input\"]";
        public static string ByPesquisar => "//*[@id=\"j_idt62:j_idt104\"]";
        public static string ByProcessando => "/html/body/div[8]";
        public static string ByViewState => "//*[@id=\"j_id1:javax.faces.ViewState:0\"]";
    }
}
