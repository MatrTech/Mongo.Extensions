using Microsoft.Extensions.DependencyInjection;

namespace MatrTech.Utilities.Mongo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDatabase<TDatabase>(this IServiceCollection source, string connectionString)
        //where TDatabase : class, DatabaseSettings, new()
        {
            return source;
        }
    }
}