using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XLogger.Options;

namespace XLogger.Adapters.MongoDB
{
    public class MongoDBLoggerOptions : LoggerOptions
    {
        /// <summary>
        /// The URL of a MongoDb database.
        /// (example: mongodb://localhost:27017/DatabaseName)
        /// </summary>
        public string DatabaseUrl { get; set; }
        
        /// <summary>
        /// Name of the collection. Default is "logs".
        /// </summary>
        public string CollectionName { get; set; }
        
        /// <summary>
        /// Option to create a capped collection. Default is false.
        /// </summary>
        public bool Capped { get; set; }

        /// <summary>
        /// Max total size in bytes of the created capped collection. Default is 100000000 bytes.
        /// </summary>
        public long MaxSize { get; set; }

        /// <summary>
        /// Max number of documents of the created capped collection. Default is 1000 documents.
        /// </summary>
        public long MaxDocuments { get; set; }

        /// <summary>
        /// Construct a <see cref="MongoDBLoggerOptions"/>.
        /// By default, the <see cref="CollectionName"/> property is set to 'logs' and <see cref="Capped"/> property is set to false.
        /// </summary>
        public MongoDBLoggerOptions() : base()
        {
            CollectionName = "logs";
            Capped = false;
            MaxSize = 100000000;
            MaxDocuments = 1000;
        }

        /// <summary>
        /// Set the MongoDB logger options based on the key/value application configuration properties.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties. See <see cref="IConfiguration"/>.</param>
        public override void ReadFromConfiguration(IConfiguration configuration)
        {
            var consoleConfiguration = configuration.GetSection("XLogger:MongoDB");
            
            var logLevel = consoleConfiguration[nameof(LogLevel)];
            if (!string.IsNullOrEmpty(logLevel))
                LogLevel = (LogLevel)int.Parse(logLevel);
            OnDemand = bool.Parse(consoleConfiguration[nameof(OnDemand)] ?? OnDemand.ToString());

            DatabaseUrl = consoleConfiguration[nameof(DatabaseUrl)] ?? DatabaseUrl;
            CollectionName = consoleConfiguration[nameof(CollectionName)] ?? CollectionName;
            Capped = bool.Parse(consoleConfiguration[nameof(Capped)] ?? Capped.ToString());
            MaxSize = long.Parse(consoleConfiguration[nameof(MaxSize)] ?? MaxSize.ToString());
            MaxDocuments = long.Parse(consoleConfiguration[nameof(MaxDocuments)] ?? MaxDocuments.ToString());
        }
    }
}