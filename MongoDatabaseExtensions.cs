using System.Linq;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPTech.Mongo.Extensions
{
    public static class MongoDatabaseExtensions
    {
        /// <summary>
        /// Creates a <see cref="List{string}"/> of all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database"></param>
        /// <returns>A <see cref="List{string}"/> that contains all collection names in the given <paramref name="database"/>.</returns>
        public static List<string> CollectionNamesAsList(this IMongoDatabase database)
        {
            return database.ListCollectionNames().ToList();
        }

        /// <summary>
        /// Creates a <see cref="Task{List{string}}"/> of all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database"></param>
        /// <returns>A <see cref="Task{List{string}}"/> that contains all collection names in the given <paramref name="database"/>.</returns>
        public static async Task<List<string>> CollectionNamesAsListAsync(this IMongoDatabase database)
        {
            return (await database.ListCollectionNamesAsync()).ToList();
        }

        /// <summary>
        /// Droppes all collections in the given <paramref name="collectionNames"/> if left empty, it will remove all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collectionNames"></param>
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

        /// <summary>
        /// Droppes all collections in the given <paramref name="collectionNames"/> if left empty, it will remove all collections in the given <paramref name="database"/>.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="collectionNames"></param>
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
