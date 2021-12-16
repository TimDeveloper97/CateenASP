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
    public class UserService : ICrud<User>
    {
        private string _collection = "user";
        private IMongoCollection<User>? _uCollection;

        protected IMongoCollection<User> UserCollection
        {
            get
            {
                if (_uCollection == null)
                    _uCollection = BaseService.Db.GetCollection<User>(_collection);
                return _uCollection;
            }
        }
        public async Task<bool> Create(User t)
        {
            if (UserCollection != null)
            {
                UserCollection.InsertOne(t);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> Delete(string id)
        {
            if (UserCollection != null)
            {
                await UserCollection.FindOneAndDeleteAsync(x => x.Id == id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<List<User>> GetAll()
        {
            var result = (await UserCollection.FindAsync(x => true)).ToListAsync();
            return await Task.FromResult(result.Result);
        }

        public async Task<bool> IsExist(string id)
        {
            if (UserCollection != null)
            {
                var user = await UserCollection.FindAsync(x => x.Id == id);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<User> Read(string id)
        {
            if (UserCollection != null)
            {
                var user = await UserCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

                return user;
            }
            return await Task.FromResult<User>(null);
        }

        [Obsolete]
        public async Task<bool> Update(User t)
        {
            var user = await UserCollection.Find(x => x.Id == t.Id).FirstOrDefaultAsync();
            if (user == null) return false;

            var filter = Builders<User>.Filter.Eq("_id", new ObjectId(t.Id));
            user.LastName = t.LastName;
            user.FirstName = t.FirstName;   
            user.Description = t.Description;
            user.Phone = t.Phone;
            //food.Password = t.Password; 

            var result = await UserCollection.ReplaceOneAsync(filter, user, options: new UpdateOptions() { IsUpsert = false });
            return result.IsAcknowledged;
        }
        public async Task<bool> Register(User user)
        {
            if(UserCollection != null)
            {
                user.Password = Common.MD5Hash(user.Password);
                user.DisplayName = user.LastName + " " + user.FirstName;
                user.Description = "normal user";
                await UserCollection.InsertOneAsync(user);
                return true;
            }
            return false;
        }
        public async Task<Response<User>> Login(string username, string password)
        {
            if(UserCollection != null)
            {
                var user = await UserCollection.Find(x => x.UserName == username).FirstOrDefaultAsync();
                if(user == null) 
                    return new Response<User>(false, "User is not exist!", null);
                if (user.Password != Common.MD5Hash(password).Substring(0,32)) 
                    return new Response<User>(false, "Password is not correct!", null);
                return new Response<User>(true, "Login successfully!", user);
            }
            return new Response<User>(false, "Connection Error!", null);
        }
    }
}
