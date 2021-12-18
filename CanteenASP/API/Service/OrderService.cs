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
    public class OrderService : ICrud<Order>
    {
        private string _collection = "order";
        private IMongoCollection<Model.Order>? _oCollection;

        protected IMongoCollection<Model.Order> OrderCollection
        {
            get
            {
                if (_oCollection == null)
                    _oCollection = BaseService.Db.GetCollection<Model.Order>(_collection);
                return _oCollection;
            }
        }

        public async Task<bool> Create(Order t)
        {
            if (OrderCollection != null)
            {
                await OrderCollection.InsertOneAsync(t);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string id)
        {
            if (OrderCollection != null)
            {
                await OrderCollection.FindOneAndDeleteAsync(x => x.Id == id);
                return true;
            }
            return false;
        }

        public async Task<List<Order>> GetAll()
        {
            var result = (await OrderCollection.FindAsync(x => true)).ToListAsync();
            return result.Result;
        }

        public async Task<bool> IsExist(string id)
        {
            if (OrderCollection != null)
            {
                var food = await OrderCollection.FindAsync(x => x.Id == id);
                return true;
            }
            return false;
        }

        public async Task<Order> GetItem(string id)
        {
            var food = await OrderCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            return food;
        }

        [Obsolete]
        public async Task<bool> Update(Order t)
        {
            var order = await OrderCollection.Find(x => x.Id == t.Id).FirstOrDefaultAsync();
            if (order == null) return false;

            var filter = Builders<Order>.Filter.Eq("_id", new ObjectId(t.Id));
            order.Food = t.Food;
            order.User = t.User;
            order.Type = t.Type;
            order.OrderTime = t.OrderTime;

            var result = await OrderCollection.ReplaceOneAsync(filter, order, options: new UpdateOptions() { IsUpsert = false });
            return result.IsAcknowledged;
        }

        public async Task<List<Order>?> GetAllByDate(DateTime date)
        {
            var all = await GetAll();
            return all.Where(x => x.OrderTime.Date == date.Date)?.ToList();
        }

        public async Task<List<Order>?> GetAllByMealTime(DateTime date, MealTime mealTime)
        {
            var all = await GetAll();
            return all.Where(x => x.OrderTime.Date == date.Date && x.Type == mealTime)?.ToList();
        }
    }
}
