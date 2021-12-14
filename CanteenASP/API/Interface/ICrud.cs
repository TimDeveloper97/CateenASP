using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Interface
{
    public interface ICrud<T>
    {
        Task<bool> Create(T t);
        Task<bool> Update(T t);
        Task<bool> Delete(string id);
        Task<T> Read(string id);
        Task<bool> IsExist(string id);
    }
}
