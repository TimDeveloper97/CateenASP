using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum MealTime
    {
        Null = -1,
        Breakfast = 1,
        Lunch,
        Dinner,
    }

    public class RangeTime
    {
        public TimeSpan Start { get; set; }
        public TimeSpan Finish { get; set; }
    }

    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public User? User { get; set; }
        public Food? Food { get; set; }
        public string? TotalPrice { get; set; }

        public DateTime OrderTime { get; set; }
    }
}
