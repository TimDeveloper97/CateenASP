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
            var tBreakfast = new RangeTime { Start = new TimeSpan(0, 0, 0), Finish = new TimeSpan(7, 0, 0) };
            var tLunch = new RangeTime { Start = new TimeSpan(0, 0, 0), Finish = new TimeSpan(11, 0, 0) };
            var tDinner = new RangeTime { Start = new TimeSpan(0, 0, 0), Finish = new TimeSpan(17, 0, 0) };

            var current = orderTime.TimeOfDay;
            if(current >= tBreakfast.Start 
                && current <= tBreakfast.Finish)
            {
                return MealTime.Breakfast;
            }
            else if(current >= tLunch.Start
                && current <= tLunch.Finish)
            { 
                return MealTime.Lunch;
            }
            else if(current >= tDinner.Start
                && current <= tDinner.Finish)
            {
                return MealTime.Dinner;
            }

            return null;
        }
    }
}
