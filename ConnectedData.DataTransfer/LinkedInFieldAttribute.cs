using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.DataTransfer
{

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class LinkedInField : Attribute
    {
        public readonly string Name;
        public LinkedInField(string name)
        {
            Name = name;
        }

        public string APIField
        {
            get
            {
                return Name.Contains(":") ? Name.Substring(0, Name.IndexOf(":")) : Name;
            }
        }
    }
}
