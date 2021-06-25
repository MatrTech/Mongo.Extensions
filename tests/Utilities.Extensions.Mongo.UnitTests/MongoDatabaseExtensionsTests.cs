using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace MatrTech.Utilities.Extensions.Mongo.UnitTests
{
    [TestClass]
    public class MongoDatabaseExtensionsTests
    {
        private static MongoClient client = null!;
        private IMongoDatabase? database;
        private IMongoDatabase? otherDatabase;

        public MongoDatabaseExtensionsTests()
        {
            client = new MongoClient("mongodb://localhost:27017");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            client.DropAllDatabases();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            database?.DropAllCollections();
            otherDatabase?.DropAllCollections();
        }

        [TestMethod]
        public void CollectionNamesAsList_CollectionInDatabase_ListContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = database.CollectionNamesAsList();

            result.Should().Contain(collectionName);
        }

        [TestMethod]
        public void CollectionNamesAsList_CollectionNotInDatabase_ListDoesNotContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();

            var result = database.CollectionNamesAsList();

            result.Should().NotContain(collectionName);
        }

        [TestMethod]
        public async Task CollectionNamesAsListAsync_CollectionInDatabase_ListShouldContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = await database.CollectionNamesAsListAsync();

            result.Should().Contain(collectionName);
        }

        [TestMethod]
        public async Task CollectionNamesAsListAsync_CollectionNotInDatabase_ListShouldNotContainsCollectionName()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();

            var result = await database.CollectionNamesAsListAsync();

            result.Should().NotContain(collectionName);
        }

        [TestMethod]
        public void DatabaseNamesAsList_DatabaseDoesExist_ShouldContain()
        {
            var databaseName = Guid.NewGuid().ToString();
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = client.DatabaseNamesAsList();

            result.Should().Contain(databaseName);
        }

        [TestMethod]
        public void DatabaseNamesAsList_DatabaseDoesNotExist_ShouldNotContain()
        {
            var databaseName = Guid.NewGuid().ToString();

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DatabaseNamesAsListAsync_DatabaseDoesNotExist_ShouldNotContain()
        {
            var databaseName = Guid.NewGuid().ToString();

            var result = await client.DatabaseNamesAsListAsync();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DatabaseNamesAsListAsync_DatabaseDoesExist_ShouldContain()
        {
            var databaseName = Guid.NewGuid().ToString();
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var result = await client.DatabaseNamesAsListAsync();

            result.Should().Contain(databaseName);
        }

        [TestMethod]
        public void DropAllCollections_DatabaseWithCollections_ShouldBeEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);

            database.DropAllCollections();

            var result = database.CollectionNamesAsList();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void DropAllCollections_RemoveOneCollection_ShouldNotContainRemovedButNotEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = database.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);
            resultBeforeDrop.Should().Contain(otherCollectionName);

            database.DropAllCollections(collectionName);

            var result = database.CollectionNamesAsList();
            result.Should().NotBeEmpty();
            result.Should().Contain(otherCollectionName);
        }

        [TestMethod]
        public async Task DropAllCollectionsAsync_DatabaseWithCollections_ShouldBeEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);

            await database.DropAllCollectionsAsync();

            var result = database.CollectionNamesAsList();
            result.Should().BeEmpty();
        }

        [TestMethod]
        public async Task DropAllCollectionsAsync_RemoveOneCollection_ShouldNotContainRemovedButNotEmpty()
        {
            database = client.GetDatabase($"test-database-{Guid.NewGuid()}");
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = database.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = database.CollectionNamesAsList();
            resultBeforeDrop.Should().Contain(collectionName);
            resultBeforeDrop.Should().Contain(otherCollectionName);

            await database.DropAllCollectionsAsync(collectionName);

            var result = database.CollectionNamesAsList();
            result.Should().NotBeEmpty();
            result.Should().Contain(otherCollectionName);
        }

        [TestMethod]
        public void DropAllDatabases_ExistingDatabase_ShouldOnlyContainAdminAndLocal()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);

            client.DropAllDatabases();

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public void DropAllDatabases_DatabaseDoesNotExist_ShouldOnlyContainAdminAndLocal()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";

            var resultBeforeDrop = client.DatabaseNamesAsList();

            resultBeforeDrop.Contains(databaseName);

            client.DropAllDatabases(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public void DropAllDatabases_DropSpecificDatabase_ShouldContainAllExceptDropped()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherDatabaseName = $"test-database-{Guid.NewGuid()}";
            otherDatabase = client.GetDatabase(otherDatabaseName);
            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = otherDatabase.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);
            resultBeforeDrop.Should().Contain(otherDatabaseName);

            client.DropAllDatabases(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
            result.Should().Contain(otherDatabaseName);
        }

        [TestMethod]
        public async Task DropAllDatabasesAsync_ExistingDatabase_ShouldNotContainDatabase()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);

            await client.DropAllDatabasesAsync();

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DropAllDatabasesAsync_DatabaseDoesNotExist_ShouldOnlyContainAdminAndLocal()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().NotContain(databaseName);

            await client.DropAllDatabasesAsync(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
        }

        [TestMethod]
        public async Task DropAllDatabasesAsync_DropSpecificDatabase_ShouldContainAllExceptDropped()
        {
            var databaseName = $"test-database-{Guid.NewGuid()}";
            database = client.GetDatabase(databaseName);
            var collectionName = Guid.NewGuid().ToString();
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(new BsonDocument { });

            var otherDatabaseName = $"test-database-{Guid.NewGuid()}";
            otherDatabase = client.GetDatabase(otherDatabaseName);
            var otherCollectionName = Guid.NewGuid().ToString();
            var otherCollection = otherDatabase.GetCollection<BsonDocument>(otherCollectionName);
            otherCollection.InsertOne(new BsonDocument { });

            var resultBeforeDrop = client.DatabaseNamesAsList();
            resultBeforeDrop.Should().Contain(databaseName);
            resultBeforeDrop.Should().Contain(otherDatabaseName);

            await client.DropAllDatabasesAsync(databaseName);

            var result = client.DatabaseNamesAsList();

            result.Should().NotContain(databaseName);
            result.Should().Contain(otherDatabaseName);
        }
    }
}