using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.LinkedIn.Service.Tests
{
    [TestFixture]
    public class ObtainConnectionsCommandHandlerTests
    {
        [Test]
        public void Can_Execute_Request_Without_Failure()
        {
            var handler = new ObtainConnectionsCommandHandler();
            var command = new ObtainUserConnectionsFromLinkedin()
            {
                AccessToken = "AQVqeezDWYfggl3PAUfDeR2s3HAWz3nelZDV6q0XDdWKhityjGV3AfmB1jtrXuzrpJOZdd4yR5n8OHiiIE7mHffRwAcSMAq9qQumZKj8n1KxEht3wiQA30M4hq9OLw55yBf4J9gtjoGyNnfwordD9Co6fb8jvxdrAfGbqEtEjR3aYO_mmmY",
                UserId = null
            };
            var results = handler.Handle(command);
            Assert.Inconclusive("working on stuff");
        }
    }
}
