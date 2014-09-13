using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Utils
{
    public enum BetweenOperationOption
    {
        /// <summary>
        /// Predicate of a &lt;= b &lt;= c
        /// </summary>
        Inclusive,
        /// <summary>
        /// Predicate of a &lt; b &lt; c
        /// </summary>
        Exclusive
    }

    public static class DateTimeExtensions
    {
        public static bool Between(this DateTime current, DateTime first, DateTime second, BetweenOperationOption option = BetweenOperationOption.Inclusive)
        {
            var result = false;
            switch (option)
            {
                case BetweenOperationOption.Inclusive:
                    result = first <= current && current <= second;
                    break;
                case BetweenOperationOption.Exclusive:
                    result = first < current && current < second;
                    break;
            }
            return result;
        }
    }

}
