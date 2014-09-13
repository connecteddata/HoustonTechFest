using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.DataTransfer
{

    public class CompanyDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class CompanyDtoIdComparer : IEqualityComparer<CompanyDto>
    {
        public bool Equals(CompanyDto x, CompanyDto y)
        {
            if (string.IsNullOrEmpty(x.Id)) return false;
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(CompanyDto obj)
        {
            if (string.IsNullOrEmpty(obj.Id)) return string.Empty.GetHashCode();
            return obj.Id.GetHashCode();
        }
    }

}
