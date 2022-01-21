using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum Size
    {
        Large = 1,
        Medium,
        Small,
    }
    public enum Type
    {
        Option = 1,
        Combo,
    }
    public class Food
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? SideDishes { get; set; }
        public MealTime MealTime { get; set; }
        public Size Size { get; set; }
        public Type Type { get; set; }
        public string? Detail { get; set; }
    }
}
