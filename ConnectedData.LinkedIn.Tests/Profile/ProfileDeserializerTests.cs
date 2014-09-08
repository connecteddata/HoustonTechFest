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
namespace ConnectedData.LinkedIn.Service.Tests.Profile
{
    [TestFixture]
    public class ProfileDeserializerTests
    {
        [Test]
        [UseReporter(typeof(BeyondCompareReporter))]
        public void Can_Deserialize()
        {
            //Arrange
            var expectedFileLocation = ConfigurationManager.AppSettings["DummyProfileFileJson"];
            var expectedFileContents = File.ReadAllText(expectedFileLocation);
            var expected = JsonConvert.DeserializeObject<DetailedPersonDto>(expectedFileContents);
            
            var dummyFileLocation = ConfigurationManager.AppSettings["DummyProfileFileXml"];
            var dummyFilefileContents = File.ReadAllText(dummyFileLocation);
            var mapper = new ProfileDeserializer();
            
            //Act
            var actual = mapper.Map(dummyFilefileContents);
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
