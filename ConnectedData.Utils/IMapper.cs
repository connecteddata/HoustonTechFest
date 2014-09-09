using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Utils
{
    public interface IMapper<in T, out V>
    {
        V Map(T t);
    }
}
