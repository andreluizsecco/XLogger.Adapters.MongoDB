using System;
using Microsoft.Extensions.DependencyInjection;
using XLogger.Adapters.MongoDB;

namespace XLogger
{
    public static class MongoDBLoggerHubExtensions
    {
        private static ILoggerHub AddMongoDB(ILoggerHub hub, IMongoDBLogger logger)
        {
            hub.Services.AddSingleton<IMongoDBLogger>(logger);
            return hub.AddLogger(logger);
        }

        public static ILoggerHub AddMongoDB(this ILoggerHub hub)
        {
            var options = new MongoDBLoggerOptions();
            options.ReadFromConfiguration(hub.Configuration);
            return AddMongoDB(hub, new MongoDBLogger(options));
        }

        public static ILoggerHub AddMongoDB(this ILoggerHub hub, Action<MongoDBLoggerOptions> options) =>
            AddMongoDB(hub, new MongoDBLogger(options));
    }
}