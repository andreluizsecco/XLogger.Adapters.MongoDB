using System;
using Microsoft.Extensions.DependencyInjection;
using XLogger.Adapters.MongoDB;

namespace XLogger
{
    public static class MongoDBLoggerHubExtensions
    {
        /// <summary>
        /// Adds the MongoDB logger to logger hub.
        /// </summary>
        /// <param name="hub">logger hub instance.</param>
        /// <param name="logger">MongoDB logger instance.</param>
        /// <returns>The logger hub.</returns>
        private static ILoggerHub AddMongoDB(ILoggerHub hub, IMongoDBLogger logger)
        {
            hub.Services.AddSingleton<IMongoDBLogger>(logger);
            return hub.AddLogger(logger);
        }

        /// <summary>
        /// Adds the new MongoDB logger with default options to logger hub.
        /// If a XLogger:MongoDB section exists in the application settings, it will be used.
        /// </summary>
        /// <param name="hub">logger hub instance.</param>
        /// <returns>The logger hub.</returns>
        public static ILoggerHub AddMongoDB(this ILoggerHub hub)
        {
            var options = new MongoDBLoggerOptions();
            if (hub.Configuration != null)
                options.ReadFromConfiguration(hub.Configuration);
            return AddMongoDB(hub, new MongoDBLogger(options));
        }

        /// <summary>
        /// Adds the new MongoDB logger with custom options to logger hub.
        /// </summary>
        /// <param name="hub">logger hub instance.</param>
        /// <param name="options">MongoDB logger options.</param>
        /// <returns>The logger hub.</returns>
        public static ILoggerHub AddMongoDB(this ILoggerHub hub, Action<MongoDBLoggerOptions> options) =>
            AddMongoDB(hub, new MongoDBLogger(options));
    }
}