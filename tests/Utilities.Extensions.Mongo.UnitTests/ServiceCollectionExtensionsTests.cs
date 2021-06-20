using FluentAssertions;
using MatrTech.Utilities.Mongo;
using MatrTech.Utilities.Mongo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MatrTech.Utilities.Extensions.Mongo.UnitTests
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        [TestMethod]
        public void AddMongoDatabase_SENARIO_EXPECTEDRESULT()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddMongoDatabase();

            var provider = services.BuildServiceProvider();

            var result = provider.GetRequiredService<TestMongoDatabase>();

            result.Should().NotBeNull();
        }

        private class TestDocument : BsonDocument { }

        private class TestMongoDatabase : MongoDatabase
        {
            public IMongoCollection<TestDocument> TestDocument { get; set; }
        }
    }
}