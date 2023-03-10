using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nNFSEautomation
{
    public class ConnectToDB
    {
        public string ConnectString = "mongodb://127.0.0.1:27017";
        public string DataBaseName = "XmlsSjcSP";
        public string CollectionName = "XMLS";
        public MongoClient MongoClient;
        public IMongoDatabase Database;
        public IMongoCollection<XmlLogin> CollectionLogin;
        public IMongoCollection<XmlClient> CollectionClient;
        public IMongoCollection<XmlInfos> CollectionXml;
        public ConnectToDB()
        {
            MongoClient = new MongoClient(ConnectString);
            Database = MongoClient.GetDatabase(DataBaseName);
        }
        public void CreateLoginsCollection()
        {
            CollectionName = "Logins";
            CollectionLogin = Database.GetCollection<XmlLogin>(CollectionName);
        }

        public void InsertLoginInDataBase(XmlLogin login)
        {
            CollectionLogin.InsertOne(login);
        }

        public void CreateClientsCollection()
        {
            CollectionName = "Clients";
            CollectionClient = Database.GetCollection<XmlClient>(CollectionName);
        }

        public void InsertClientsInDataBase(XmlClient client)
        {
            CollectionClient.InsertOne(client);
        }

        public void CreateXmlCollection()
        {
            CollectionName = "Xmls";
            CollectionXml = Database.GetCollection<XmlInfos>(CollectionName);
        }

        public void InsertXmlInDataBase(XmlInfos xml)
        {
            CollectionXml.InsertOne(xml);
        }
    }
}
