using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yamlscaffold.CLI
{
    public static class IEnumerableExtensions
    {
        public static string GetItemsAsString<T>(this IEnumerable<T> list, string seperator)
        {
            var toReturn = "";

            foreach(T item in list)
            {
                if(EqualityComparer<T>.Default.Equals(list.Last(),item))
                {
                    toReturn += $"{item}";
                }
                else
                {
                    toReturn += $"{item} + {seperator}";

                }
            }

            return toReturn;

        }   
    }
}
