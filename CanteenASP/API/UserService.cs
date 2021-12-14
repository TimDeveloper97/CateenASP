using API.Interface;
using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class UserService : ICrud<User>
    {
        private string _collection = "user";
        public async Task<bool> Create(User t)
        {
            var uService = BaseService.Db.GetCollection<User>(_collection);
            if (uService != null)
            {
                uService.InsertOne(t);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> Delete(string id)
        {
            var uService = BaseService.Db.GetCollection<User>(_collection);
            if (uService != null)
            {
                await uService.FindOneAndDeleteAsync(x => x.Id == id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> IsExist(string id)
        {
            var uService = BaseService.Db.GetCollection<User>(_collection);
            if (uService != null)
            {
                var user = await uService.FindAsync(x => x.Id == id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<User> Read(string id)
        {
            var uService = BaseService.Db.GetCollection<User>(_collection);
            if (uService != null)
            {
                var user = await uService.FindAsync(x => x.Id == id);

                return await Task.FromResult((User)user);
            }
            return await Task.FromResult<User>(null);
        }

        public async Task<bool> Update(User t)
        {
            var uService = BaseService.Db.GetCollection<User>(_collection);
            if (uService != null)
            {
                var result = await uService.UpdateOneAsync(x => x.Id == t.Id,
                     Builders<User>.Update.Set(z => z, t));
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
