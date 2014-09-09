using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.Domain
{
    public abstract class Nothing { }

    
    public class Interest : Nothing
    {
        public string Name { get; set; }
    }
}
