using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public static class Common
    {
        static RangeTime _tBreakfast = new RangeTime { Start = new TimeSpan(20, 0, 0), Finish = new TimeSpan(24, 0, 0) };
        static RangeTime _tLunch = new RangeTime { Start = new TimeSpan(6, 0, 0), Finish = new TimeSpan(10, 0, 0) };
        static RangeTime _tDinner = new RangeTime { Start = new TimeSpan(12, 0, 0), Finish = new TimeSpan(16, 0, 0) };

        public static string? MD5Hash(string input)
        {
            if(input == null) return null;

            MD5 md5 = MD5.Create();
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static MealTime TimeToEnum(DateTime orderTime)
        {
            var current = orderTime.TimeOfDay;
            if(current >= _tBreakfast.Start 
                && current <= _tBreakfast.Finish)
            {
                return MealTime.Breakfast;
            }
            else if(current >= _tLunch.Start
                && current <= _tLunch.Finish)
            { 
                return MealTime.Lunch;
            }
            else if(current >= _tDinner.Start
                && current <= _tDinner.Finish)
            {
                return MealTime.Dinner;
            }

            return MealTime.Null;
        }

        public static RangeTime? EnumToTime(MealTime mealTime)
        {
            switch (mealTime)
            {
                case MealTime.Null:
                    break;

                case MealTime.Breakfast:
                    return _tBreakfast;

                case MealTime.Lunch:
                    return _tLunch;

                case MealTime.Dinner:
                    return _tDinner;
            }

            return null;
        }
    }
}