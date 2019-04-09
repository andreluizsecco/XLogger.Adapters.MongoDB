using System.Linq;
using MongoDB.Driver;

namespace XLogger.Adapters.MongoDB
{
    public static class MongoDBExtensions
    {
        public static IMongoCollection<TDocument> GetCollection<TDocument>(this IMongoDatabase database, MongoDBLoggerOptions options)
        {
            var collection = database.GetCollection<TDocument>(options.CollectionName);
            if (!collection.Exists())
            {
                var collectionOptions = new CreateCollectionOptions();
                collectionOptions.Capped = options.Capped;
                collectionOptions.MaxSize = options.MaxSize;
                collectionOptions.MaxDocuments = options.MaxDocuments;

                database.CreateCollection(options.CollectionName, collectionOptions);
            }
            return collection;
        }

        public static bool Exists<TDocument>(this IMongoCollection<TDocument> collection)
        {
            return collection.Database.ListCollectionNames()
                .ToList().Any(collectionName => collectionName.Equals(collection.CollectionNamespace.CollectionName));
        }
    }
}