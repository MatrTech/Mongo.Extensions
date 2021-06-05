using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;

namespace MPTech.Mongo.Extensions
{
    public static class MongoDatabaseExtensions
    {
        public static List<string> CollectionNamesAsList(this IMongoDatabase database)
        {
            return database.ListCollectionNames().ToList();
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
    }
}
