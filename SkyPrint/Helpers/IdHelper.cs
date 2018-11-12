using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyPrint.Helpers
{
    public class IdHelper: IIdHelper
    {
        public string CutIdBeforeFirstLetter(string id)
        {
            var allowedChars = GetNumbers().Concat(GetSymbols());
            var length = id.Length;

            for (int i = 0; i < length; i++)
            {
                if (!allowedChars.Contains(id[i]))
                {
                    return new string(id.Take(i).ToArray()); 
                }
            }
            return id;
        }

        private IEnumerable<char> GetNumbers()
        {
            ICollection<char> data = new List<char>();

            for (int i = 0; i <= 9; i++)
            {
                data.Add((char)(i + '0'));
            }
            return data;
        }

        private IEnumerable<char> GetSymbols()
        {
            ICollection<char> data = new List<char>()
            {
                '_', '-'
            };

            return data;
        }
    }
}
