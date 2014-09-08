using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ApprovalTests;
using ObjectApproval;
using ApprovalTests.Reporters;
using ConnectedData.DataTransfer;

namespace ConnectedData.LinkedIn.Tests
{
    [TestFixture]
    public class ConnectionsDeserializerTests
    {
        [Test]
        [UseReporter(typeof(BeyondCompareReporter))]
        public void Can_Deserialize()
        {
            //Arrange
            var dummyFileLocation = ConfigurationManager.AppSettings["DummyConnectionsFileXml"];
            var fileContents = File.ReadAllText(dummyFileLocation);
            var mapper = new ConnectionsDeserializer();
            //Act
            var actual = mapper.Map(fileContents);
            //Assert
            try
            {
                actual.ShouldBeEquivalentTo(new DetailedPersonDto());
            }
            catch
            {
                ObjectApprover.VerifyWithJson(actual);
            }

        }
    }
}
