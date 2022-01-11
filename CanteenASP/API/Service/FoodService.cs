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
        private IMongoCollection<Model.Food>? _fCollection;

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
                await FoodCollection.InsertOneAsync(t);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string id)
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

        public async Task<bool> IsExist(string id)
        {
            if (FoodCollection != null)
            {
                var food = await FoodCollection.FindAsync(x => x.Id == id);
                return true;
            }
            return false;
        }

        public async Task<Model.Food> GetItem(string id)
        {
            var food = await FoodCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            return food;
        }

        [Obsolete]
        public async Task<bool> Update(Model.Food t)
        {
            var food = await FoodCollection.Find(x => x.Id == t.Id).FirstOrDefaultAsync();
            if (food == null) return false;

            var filter = Builders<Food>.Filter.Eq("_id", new ObjectId(t.Id));
            food.Name = t.Name;
            food.Description = t.Description;
            food.Price = t.Price;
            food.SideDishes = t.SideDishes;
            food.Image = t.Image;
            food.MealTime = t.MealTime;
            food.Size = t.Size;
            food.Type = t.Type;
            food.Detail = t.Detail;

            var result = await FoodCollection.ReplaceOneAsync(filter, food, new UpdateOptions { IsUpsert = true });
            return result.IsAcknowledged;
        }
        public async Task<List<Food>> GetFoodByMealTime(MealTime mealTime)
        {
            var foods = await FoodCollection.Find(x => x.MealTime == mealTime).ToListAsync();
            return foods;
        }
        public Task UpdateData()
        {
            var filterDefinition = Builders<Food>.Filter.Where(w => w.Name != null);
            var updateDefinition = Builders<Food>.Update
                .Set(d => d.Size, Size.Medium)
                .Set(d => d.Detail, "Detail")
                .Set(d => d.Type, Model.Type.Combo);
            FoodCollection.UpdateMany(filterDefinition, updateDefinition);
            return Task.CompletedTask;
        }
    }
}
