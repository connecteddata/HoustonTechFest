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
using Newtonsoft.Json;
using ConnectedData.LinkedIn.Connections;
using Newtonsoft.Json.Bson;

namespace ConnectedData.LinkedIn.Tests
{
    [TestFixture]
    public class ConnectionsDeserializerTests
    {
        [Test]
        [UseReporter(typeof(BeyondCompareReporter))]
        public void Can_Deserialize_Connections()
        {
            //Arrange
            var expectedFileLocation = ConfigurationManager.AppSettings["DummyConnectionsFileJson"];
            var expectedFileContents = File.ReadAllText(expectedFileLocation);
            var expected = JsonConvert.DeserializeObject<IEnumerable<PersonDto>>(expectedFileContents);

            var dummyFileLocation = ConfigurationManager.AppSettings["DummyConnectionsFileXml"];
            var fileContents = File.ReadAllText(dummyFileLocation);
            var mapper = new ConnectionsDeserializer();
            //Act
            var actual = mapper.Map(fileContents);

            var di = (new FileInfo(expectedFileLocation)).Directory;
            
            //Assert
            try
            {
                actual.ShouldBeEquivalentTo(expected);
            }
            catch
            {
                ObjectApprover.VerifyWithJson(actual);
            }

        }
    }
}
