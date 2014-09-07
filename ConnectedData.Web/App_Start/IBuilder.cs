using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectedData.Web.App_Start
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}
