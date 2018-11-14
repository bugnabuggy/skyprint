using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Internal;

namespace SkyPrint.Helpers
{
    public class IdHelper: IIdHelper
    {
        // Returns cutted before non-allowed char part of recieved "id"
        public string CutIdBeforeFirstLetter(string id)
        {
            var allowedChars = GetNumbers().Concat(GetSymbols());

            int cutPos = 0;

            while (allowedChars.Any(x => x == id[cutPos]))
            {
                cutPos++;
            }

            var result = new string(id.SkipLast(id.Length - cutPos).ToArray());

            return result;
        }

        // Array of allowed numbers
        private IEnumerable<char> GetNumbers()
        {
            ICollection<char> data = new List<char>();

            for (int i = 0; i <= 9; i++)
            {
                data.Add((char)(i + '0'));
            }
            return data;
        }

        // Array of allowed symbols
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
