using ConnectedData.DataTransfer;
using ConnectedData.LinkedIn.Profiles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ApprovalTests;
using ApprovalTests.Reporters;
using ObjectApproval;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using ConnectedData.LinkedIn.Connections;

namespace ConnectedData.LinkedIn.Service.Tests.Profile
{
    [TestFixture]
    public class ObtainConnectionQueryHandlerTests
    {
        [Test]
        [UseReporter(typeof(BeyondCompareReporter))]
        public void Can_Obtain_Connections()
        {
            //Arrange
            var filePath = ConfigurationManager.AppSettings["DummyConnectionsFileJson"];
            var fileContents = File.ReadAllText(filePath);
            var expected = JsonConvert.DeserializeObject<IEnumerable<PersonDto>>(fileContents);
            var accessToken = "AQVqeezDWYfggl3PAUfDeR2s3HAWz3nelZDV6q0XDdWKhityjGV3AfmB1jtrXuzrpJOZdd4yR5n8OHiiIE7mHffRwAcSMAq9qQumZKj8n1KxEht3wiQA30M4hq9OLw55yBf4J9gtjoGyNnfwordD9Co6fb8jvxdrAfGbqEtEjR3aYO_mmmY";
            var query = new ObtainConnectionsQuery(accessToken);
            var handler = new ObtainConnectionsQueryHandler();
            //Act
            var actual = handler.Handle(query);
            //Assert
            try
            {
                expected.ShouldBeEquivalentTo(actual);
            }
            catch
            {
                ObjectApprover.VerifyWithJson(actual);
            }
        }
    }
}
