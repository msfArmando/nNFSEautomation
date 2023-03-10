using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class NodePaths
    {
        public static string NfeInXml = "//xmlF:Nfse";
        public static string Numero = "//nfeF:InfNfse/nfeF:Numero";
        public static string CodigoVerificacao = "//nfeF:InfNfse/nfeF:CodigoVerificacao";
        public static string DataEmissao = "//nfeF:InfNfse/nfeF:DataEmissao";
        public static string ValorServico = "//nfeF:InfNfse/nfeF:Servico/nfeF:Valores/nfeF:ValorServicos";
        public static string CnpjPrestador = "//nfeF:InfNfse/nfeF:PrestadorServico/nfeF:IdentificacaoPrestador/nfeF:Cnpj";
        public static string RazaoSocialPrestador = "//nfeF:InfNfse/nfeF:PrestadorServico/nfeF:RazaoSocial";
        public static string CnpjTomador = "//nfeF:InfNfse/nfeF:TomadorServico/nfeF:IdentificacaoTomador/nfeF:CpfCnpj/nfeF:Cnpj";
        public static string CpfTomador = "//nfeF:InfNfse/nfeF:TomadorServico/nfeF:IdentificacaoTomador/nfeF:CpfCnpj/nfeF:Cpf";
        public static string RazaoSocialTomador = "//nfeF:InfNfse/nfeF:TomadorServico/nfeF:RazaoSocial";

    }
}
