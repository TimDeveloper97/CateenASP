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
        public async Task<bool> Create(Food t)
        {
            var fService = BaseService.Db.GetCollection<Food>(_collection);
            if (fService != null)
            {
                fService.InsertOne(t);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> Delete(string id)
        {
            var fService = BaseService.Db.GetCollection<Food>(_collection);
            if (fService != null)
            {
                await fService.FindOneAndDeleteAsync(x => x.Id == id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> IsExist(string id)
        {
            var fService = BaseService.Db.GetCollection<Food>(_collection);
            if (fService != null)
            {
                var food = await fService.FindAsync(x => x.Id == id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<Food> Read(string id)
        {
            var fService = BaseService.Db.GetCollection<Food>(_collection);
            if (fService != null)
            {
                var food = await fService.FindAsync(x => x.Id == id);

                return await Task.FromResult((Food)food);
            }
            return await Task.FromResult<Food>(null);
        }

        public async Task<bool> Update(Food t)
        {
            var fService = BaseService.Db.GetCollection<Food>(_collection);
            if (fService != null)
            {
                var result = await fService.UpdateOneAsync(x => x.Id == t.Id,
                     Builders<Food>.Update.Set(z => z, t));
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
