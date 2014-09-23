using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConnectedData.Utils.Tests
{
    using ConnectedData.Utils;

    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [TestCase(
            ""
            ,Description=".IsEmpty() Extension method test on an empty list, which should return true",
            ExpectedResult = true
            )
        ]
        [TestCase(
            "foo, bar"
            ,Description=".IsEmpty() Extension method test on an non-empty list, which should return false",
            ExpectedResult = false
            )
        ]
        [TestCase(
            null
            , Description = ".IsEmpty() Extension method test on an null object, which should throw a ArgumentNullException",
            ExpectedException = typeof(ArgumentNullException)
            )
        ]
        public bool IsEmptyTests(string commaSeparatedStringInput)
        {
            var input = DelimitedStringToEnumerable(commaSeparatedStringInput, ',');

            return input.IsEmpty();
        }

        [TestCase(
            null
            , Description = ".IsNullOrEmpty() Extension method test on a null object, which should return true",
            ExpectedResult = true
            )
        ]
        [TestCase(
            ""
            , Description = ".IsNullOrEmpty() Extension method test on an empty list, which should return true",
            ExpectedResult = true
            )
        ]
        [TestCase(
            "foo, bar"
            , Description = ".IsNullOrEmpty() Extension method test on an non-empty list, which should return false",
            ExpectedResult = false
            )
        ]
        public bool IsNullOrEmptyTests(string commaSeparatedStringInput)
        {
            var input = DelimitedStringToEnumerable(commaSeparatedStringInput,',');

            return input.IsNullOrEmpty();
        }

        [TestCase(
            "foo, bar"
            , Description = ".IsNotEmpty() Extension method test on an non-empty list, which should return true",
            ExpectedResult = true
            )
        ]
        [TestCase(
            ""
            , Description = ".IsNotEmpty() Extension method test on an empty list, which should return false",
            ExpectedResult = false
            )
        ]
        [TestCase(
            null
            , Description = ".IsNotEmpty() Extension method test on a null object, which should throw a AgrumentNullException",
            ExpectedException = typeof(ArgumentNullException)
            )
        ]
        public bool IsNotEmptyTests(string commaSeparatedStringInput)
        {
            var input = DelimitedStringToEnumerable(commaSeparatedStringInput, ',');

            return input.IsNotEmpty();
        }



        private IEnumerable<string> DelimitedStringToEnumerable(string input, char delimiter)
        {
            if (null == input) return null;
            return input
                    .Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim());
        }
    }
}
