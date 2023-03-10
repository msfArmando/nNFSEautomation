using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class XmlClient
    {
        [BsonId]
        public static string IdClient { get; set; }
        public string RefLoginId { get; set; }
        public string NameClient { get; set; }
        public string CpfCnpjClient { get; set; }
        public XmlClient()
        {
            IdClient = Guid.NewGuid().ToString();
            RefLoginId = XmlLogin.IdLogin;
        }
    }
}
