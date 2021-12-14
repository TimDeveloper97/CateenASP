using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Interface
{
    public interface ICrud<T>
    {
        Task<List<T>> GetAll();
        Task<bool> Create(T t);
        Task<bool> Update(T t);
        Task<bool> Delete(ObjectId id);
        Task<T> Read(ObjectId id);
        Task<bool> IsExist(ObjectId id);
    }
}
