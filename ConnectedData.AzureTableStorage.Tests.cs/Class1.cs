using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedData.AzureTableStorage.Tests.cs
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Configuration;

    [TestFixture]
    public class Class1
    {
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;

        private List<string> TEST_TABLES = new List<string>() { "test_profiles", "test_connections", "test_interests" };

        [TestFixtureSetUp]
        public void AllTestsInit()
        {
            _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureTablStorageConnectionString"]);
            _tableClient = _storageAccount.CreateCloudTableClient();
        }
        [SetUp]
        
        [Test]
        public void Can_Create_Table()
        {
            foreach (var tableName in TEST_TABLES)
            {
                CloudTable table = _tableClient.GetTableReference("techfest");
                Console.Write(_storageAccount.TableStorageUri);
                Assert.True(table.Exists());
            }
        }

        [Test]
        public void Did_I_creat_A_blob()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["AzureTablStorageConnectionString"]
            );

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("techfest");

            // Create the container if it doesn't already exist.
            Assert.True(container.CreateIfNotExists());
        }

        [TestFixtureTearDown]
        public void AllTestsDown()
        {

        }
    }
}
