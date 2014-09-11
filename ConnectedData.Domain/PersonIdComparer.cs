using ConnectedData.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectedData.Domain
{
    public class PersonIdComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y)
        {
            if (string.IsNullOrEmpty(x.Id)) return false;
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Person obj)
        {
            if (string.IsNullOrEmpty(obj.Id)) return string.Empty.GetHashCode();
            return obj.Id.GetHashCode();
        }
    }

    public class LocationNameComparer : IEqualityComparer<Location>
    {

        public bool Equals(Location x, Location y)
        {
            if (string.IsNullOrEmpty(x.Name)) return false;
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(Location obj)
        {
            if (string.IsNullOrEmpty(obj.Name)) return string.Empty.GetHashCode();
            return obj.Name.GetHashCode();
        }
    }

    public class CountryCodeComparer : IEqualityComparer<Country>

    {

        public bool Equals(Country x, Country y)
        {
            if (string.IsNullOrEmpty(x.CountryCode)) return false;
            return x.CountryCode.Equals(y.CountryCode);
        }

        public int GetHashCode(Country obj)
        {
            if (string.IsNullOrEmpty(obj.CountryCode)) return string.Empty.GetHashCode();
            return obj.CountryCode.GetHashCode();
        }
    }
}
