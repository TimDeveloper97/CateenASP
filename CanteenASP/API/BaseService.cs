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
        public BaseService()
        {
            var client = new MongoClient("mongodb + srv://timdeveloper:<password>@cluster0.6ejvs.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
        }
    }
}
