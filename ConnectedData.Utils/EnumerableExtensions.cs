using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Utils
{
        public static class EnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> current)
        {
            return 0 == current.Count();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> current)
        {
            return (null == current) ? true : current.IsEmpty();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> current)
        {
            return !current.IsEmpty();
        }
    }
}

