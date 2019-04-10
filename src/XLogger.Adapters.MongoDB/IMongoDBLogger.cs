namespace XLogger.Adapters.MongoDB
{
    public interface IMongoDBLogger : ILogger
    {
        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        void Write<TData>(TData data);
    }
}