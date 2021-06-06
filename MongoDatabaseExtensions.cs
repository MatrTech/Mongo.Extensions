using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPTech.Mongo.Extensions
{
    public static class MongoDatabaseExtensions
    {
        public static List<string> CollectionNamesAsList(this IMongoDatabase database)
        {
            return database.ListCollectionNames().ToList();
        }

        public static async Task<List<string>> CollectionNamesAsListAsync(this IMongoDatabase database)
        {
            return (await database.ListCollectionNamesAsync()).ToList();
        }

        public static void DropAllCollections(this IMongoDatabase database, params string[] collectionNames)
        {
            if (collectionNames.Any())
            {
                collectionNames.ToList()
                    .ForEach(collectionName => database.DropCollection(collectionName));

                return;
            }

            database.CollectionNamesAsList()
                .ForEach(collectionName => database.DropCollection(collectionName));
        }

        public static async Task DropAllCollectionsAsync(this IMongoDatabase database, params string[] collectionNames)
        {
            if (collectionNames.Any())
            {
                database.DropAllCollections(collectionNames);
                return;
            }

            var allCollectionNames = await database.CollectionNamesAsListAsync();
            allCollectionNames.ForEach(collectionName
                => database.DropCollectionAsync(collectionName));
        }
    }
}
