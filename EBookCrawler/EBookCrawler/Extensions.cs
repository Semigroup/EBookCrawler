using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
   public static class Extensions
    {
        public static string SumText<T>(this IEnumerable<T> list, string delimeter)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (var item in list)
            {
                if (first)
                    first = false;
                else
                    sb.Append(delimeter);
                sb.Append(item);
            }
            return sb.ToString();
        }

        public static IEnumerable<B> Map<A,B>(this IEnumerable<A> elements, Func<A,B> mapping)
        {
            foreach (var a in elements)
                yield return mapping(a);
        }
    }
}
