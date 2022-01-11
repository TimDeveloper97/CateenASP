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
        private static string _connectionString = "mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false";
        
        private static string _databaseName = "CanteenASP";

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
