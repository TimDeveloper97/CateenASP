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
    public class FoodService : ICrud<Model.Food>
    {
        private string _collection = "food";
        private IMongoCollection<Model.Food> _fCollection;

        protected IMongoCollection<Model.Food> FoodCollection 
        {
            get
            {
                if(_fCollection == null)
                    _fCollection = BaseService.Db.GetCollection<Model.Food>(_collection);
                return _fCollection;    
            }
        }

        public async Task<bool> Create(Model.Food t)
        {
            if (FoodCollection != null)
            {
                FoodCollection.InsertOne(t);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(ObjectId id)
        {
            if (FoodCollection != null)
            {
                await FoodCollection.FindOneAndDeleteAsync(x => x.Id == id);
                return true;
            }
            return false;
        }

        public async Task<List<Model.Food>> GetAll()
        {
            var result = (await FoodCollection.FindAsync(x => true)).ToListAsync();
            return result.Result;
        }

        public async Task<bool> IsExist(ObjectId id)
        {
            if (FoodCollection != null)
            {
                var food = await FoodCollection.FindAsync(x => x.Id == id);
                return true;
            }
            return false;
        }

        public async Task<Model.Food> Read(ObjectId id)
        {
            if (FoodCollection != null)
            {
                var food = await FoodCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

                return food;
            }
            return null;
        }

        public async Task<bool> Update(Model.Food t)
        {
            if (FoodCollection != null)
            {
                var food = await FoodCollection.Find(x => x.Id == t.Id).FirstOrDefaultAsync();
                if (food == null)
                {
                    return false;
                }
                var filter = Builders<Food>.Filter.Eq("_id", t.Id);
                
                food.Name = t.Name;
                food.Description = t.Description;
                food.Price = t.Price;
                food.SideDishes = t.SideDishes;  

                var result = await FoodCollection.ReplaceOneAsync(filter, food);
                return true;
            }
            return false;
        }
    }
}
