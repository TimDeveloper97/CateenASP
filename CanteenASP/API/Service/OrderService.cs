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
                //if(t.Food != null && t.Food.MealTime != Common.TimeToEnum(t.OrderTime))
                //{
                //    return false;
                //}

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
            return all.Where(x => x.OrderTime.Date == date.Date && x.Food.MealTime == mealTime)?.ToList();
        }

        public Task<string> ExportCsvWithList(List<Order> orders)
        {
            var csv = new StringBuilder();

            //header
            csv.AppendLine("ID, Display Name, Phone, Food Name, Side Dishes, Meal Time, Size, Type, Detail, Price, Order Time");

            //body
            int index = 1;
            foreach (var order in orders)
            {
                var newLine = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", index++, order.User.DisplayName, order.User.Phone,
                                            order.Food.Name, order.Food.SideDishes, order.Food.MealTime, order.Food.Size, 
                                            order.Food.Type, order.Food.Detail, order.Food.Price, order.OrderTime);
                csv.AppendLine(newLine);
            }

            return Task.FromResult(csv.ToString());
        }

        public async Task<string> ExportCsv()
        {
            return await ExportCsvWithList(await GetAll());
        }

        public async Task<string> ExportCsv(DateTime date)
        {
            var result = await GetAllByDate(date);
            return await ExportCsvWithList(result);
        }
        public async Task<List<Order>> GetOrdersByUser(string userId)
        {
            var orders = await OrderCollection.Find(x => x.User.Id == userId).ToListAsync();
            return orders;
        }
        public async Task<bool> MealTimeIsExist(string userId, MealTime mealTime)
        {
            var orders = await GetAll();
            var order = orders.Where(x => x.User.Id == userId && x.Food != null && x.Food.MealTime == mealTime 
            && x.OrderTime.Date == DateTime.Now.Date).FirstOrDefault();
            return order == null ? false : true;
        }
        
        public async Task<bool> MealTimeIsExist(string userId, List<Food> foods)
        {
            var orders = await GetAll();
            var mealTime = foods[0].MealTime;
            var order = orders.Where(x => x.User.Id == userId && x.Foods != null && x.Foods[0].MealTime == mealTime
            && x.OrderTime.Date == DateTime.Now.Date).FirstOrDefault();

            return order == null ? false : true;
        }

        
    }
}
