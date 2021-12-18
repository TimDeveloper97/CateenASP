using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
        public bool Success { get; set; }
        public Response(bool success, string message, T result)
        {
            Success = success;
            Message = message;
            Result = result;
        }
    }
}
