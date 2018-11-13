using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Helpers
{
    public class Responses
    {
        private static string[] responses = 
        {
            "Макет одобрен",
            "Правка",
            "Заказ звонка дизайнера"
        };

        public static string GetResponse(int id)
        {
            return responses[id];
        }
    }
}
