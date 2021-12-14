using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    internal class BaseService
    {
        private static string _connectionString = "mongodb + srv://timdeveloper:Duyanh1997@cluster0.6ejvs.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
        private static string _databaseName = "CateenASP";

        private static IMongoDatabase? _db;

        public static IMongoDatabase Db
        {
            get
            {
                if (_db == null)
                {
                    var client = new MongoClient(_connectionString);
                    _db = client.GetDatabase(_databaseName);
                }
                return _db;
            }
        }
    }
}
