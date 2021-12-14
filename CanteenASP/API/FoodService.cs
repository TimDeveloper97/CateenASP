using API.Interface;
using Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class FoodService : ICrud<Food>
    {
        private string _collection = "food";
        private IMongoCollection<Food> _fCollection;

        protected IMongoCollection<Food> FoodCollection 
        {
            get
            {
                if(_fCollection == null)
                    _fCollection = BaseService.Db.GetCollection<Food>(_collection);
                return _fCollection;    
            }
        }

        public async Task<bool> Create(Food t)
        {
            t.Id = new ObjectId();
            if (FoodCollection != null)
            {
                FoodCollection.InsertOne(t);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string id)
        {
            if (FoodCollection != null)
            {
                await FoodCollection.FindOneAndDeleteAsync(x => x.Id == new ObjectId(id));
                return true;
            }
            return false;
        }

        public async Task<List<Food>> GetAll()
        {
            var result = (await FoodCollection.FindAsync(x => true)).ToListAsync();
            return result.Result;
        }

        public async Task<bool> IsExist(string id)
        {
            if (FoodCollection != null)
            {
                var food = await FoodCollection.FindAsync(x => x.Id == new ObjectId(id));
                return true;
            }
            return false;
        }

        public async Task<Food> Read(string id)
        {
            if (FoodCollection != null)
            {
                var food = await FoodCollection.FindAsync(x => x.Id == new ObjectId(id));

                return (Food)food;
            }
            return null;
        }

        public async Task<bool> Update(Food t)
        {
            var fService = BaseService.Db.GetCollection<Food>(_collection);
            if (FoodCollection != null)
            {
                var result = await FoodCollection.UpdateOneAsync(x => x.Id == t.Id,
                     Builders<Food>.Update.Set(z => z, t));
                return true;
            }
            return false;
        }
    }
}
