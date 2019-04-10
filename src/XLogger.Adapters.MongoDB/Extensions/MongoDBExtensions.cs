using System.Linq;
using MongoDB.Driver;

namespace XLogger.Adapters.MongoDB
{
    public static class MongoDBExtensions
    {
        /// <summary>
        /// Gets a collection. If not exists, creates the collection with the specified options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="database">MongoDB database.</param>
        /// <param name="options">MongoDB logger options.</param>
        /// <returns>An implementation of a collection.</returns>
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

        /// <summary>
        /// Checks if a collection exists in the database.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="collection">MongoDB collection instance.</param>
        /// <returns>True if exists.</returns>
        public static bool Exists<TDocument>(this IMongoCollection<TDocument> collection)
        {
            return collection.Database.ListCollectionNames()
                .ToList().Any(collectionName => collectionName.Equals(collection.CollectionNamespace.CollectionName));
        }
    }
}