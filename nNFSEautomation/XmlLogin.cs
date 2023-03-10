using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class XmlLogin
    {
        [BsonId]
        public static string IdLogin { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public XmlLogin()
        {
            IdLogin = Guid.NewGuid().ToString();
        }
    }
}
