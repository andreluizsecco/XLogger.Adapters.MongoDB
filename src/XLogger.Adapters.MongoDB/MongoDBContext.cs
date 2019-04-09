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
                new ConventionPack{ new IgnoreIfDefaultConvention(true) }, f => true);
        }

        public void InsertOne<TDocument>(TDocument document)
        {
            var collection = _database.GetCollection<TDocument>(_loggerOptions);
            collection.InsertOne(document);
        }
    }
}