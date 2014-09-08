using ConnectedData.DataTransfer;
using ConnectedData.LinkedIn.Profile;
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
            var filePath = ConfigurationManager.AppSettings["DummyProfileFileJson"];
            var fileContents = File.ReadAllText(filePath);
            var expected = JsonConvert.DeserializeObject<DetailedPersonDto>(fileContents);
            var accessToken = "AQVqeezDWYfggl3PAUfDeR2s3HAWz3nelZDV6q0XDdWKhityjGV3AfmB1jtrXuzrpJOZdd4yR5n8OHiiIE7mHffRwAcSMAq9qQumZKj8n1KxEht3wiQA30M4hq9OLw55yBf4J9gtjoGyNnfwordD9Co6fb8jvxdrAfGbqEtEjR3aYO_mmmY";
            string userId = null;
            var query = new ObtainProfileQuery(accessToken, userId);
            var handler = new ObtainLinkedInProfileQueryHandler();
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
