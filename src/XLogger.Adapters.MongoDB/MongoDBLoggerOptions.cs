using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XLogger.Options;

namespace XLogger.Adapters.MongoDB
{
    public class MongoDBLoggerOptions : LoggerOptions
    {
        public string DatabaseUrl { get; set; }
        public string CollectionName { get; set; }
        public bool Capped { get; set; }
        public long MaxSize { get; set; }
        public long MaxDocuments { get; set; }

        public MongoDBLoggerOptions() : base()
        {
            CollectionName = "logs";
            Capped = false;
        }

        public override void ReadFromConfiguration(IConfiguration configuration)
        {
            var consoleConfiguration = configuration.GetSection("XLogger:MongoDB");
            
            var logLevel = consoleConfiguration[nameof(LogLevel)];
            if (!string.IsNullOrEmpty(logLevel))
                this.LogLevel = (LogLevel)int.Parse(logLevel);
            this.OnDemand = bool.Parse(consoleConfiguration[nameof(OnDemand)] ?? this.OnDemand.ToString());

            this.DatabaseUrl = consoleConfiguration[nameof(DatabaseUrl)] ?? this.DatabaseUrl;
            this.CollectionName = consoleConfiguration[nameof(CollectionName)] ?? this.CollectionName;
            this.Capped = bool.Parse(consoleConfiguration[nameof(Capped)] ?? this.Capped.ToString());
            this.MaxSize = long.Parse(consoleConfiguration[nameof(MaxSize)] ?? this.MaxSize.ToString());
            this.MaxDocuments = long.Parse(consoleConfiguration[nameof(MaxDocuments)] ?? this.MaxDocuments.ToString());
        }
    }
}