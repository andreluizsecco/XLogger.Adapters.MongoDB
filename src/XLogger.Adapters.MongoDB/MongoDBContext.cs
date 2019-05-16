using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace XLogger.Adapters.MongoDB
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoDBLoggerOptions _loggerOptions;

        public MongoDBContext(MongoDBLoggerOptions options)
        {
            _loggerOptions = options;
            
            var mongoUrl = MongoUrl.Create(options.DatabaseUrl);
            _database = new MongoClient(mongoUrl)
                .GetDatabase(mongoUrl.DatabaseName);

            ConventionRegistry.Register("IgnoreIfDefault", 
                new ConventionPack{ new IgnoreIfDefaultConvention(true) }, t => true);
                
            ConventionRegistry.Register("EnumStringConvention", 
                new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, t => true);
        }

        /// <summary>
        /// Inserts a single document.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="document">the document.</param>
        public void InsertOne<TDocument>(TDocument document)
        {
            var collection = _database.GetCollection<TDocument>(_loggerOptions);
            collection.InsertOne(document);
        }

        /// <summary>
        /// Inserts a single document.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="document">the document.</param>
        public async Task InsertOneAsync<TDocument>(TDocument document)
        {
            var collection = await _database.GetCollectionAsync<TDocument>(_loggerOptions);
            await collection.InsertOneAsync(document);
        }

        /// <summary>
        /// Gets the log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of documents.</returns>
        public IEnumerable<TDocument> Get<TDocument>(Expression<Func<TDocument, bool>> filter, FindOptions options = null)
        {
            var collection = _database.GetCollection<TDocument>(_loggerOptions);
            return collection.Find(filter, options).ToList();
        }

        /// <summary>
        /// Gets the log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of documents.</returns>
        public async Task<IEnumerable<TDocument>> GetAsync<TDocument>(Expression<Func<TDocument, bool>> filter, FindOptions<TDocument, TDocument> options = null)
        {
            var collection = await _database.GetCollectionAsync<TDocument>(_loggerOptions);
            return (await collection.FindAsync(filter, options)).ToList();
        }
    }
}