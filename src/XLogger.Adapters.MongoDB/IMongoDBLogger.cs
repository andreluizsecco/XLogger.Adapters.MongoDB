namespace XLogger.Adapters.MongoDB
{
    public interface IMongoDBLogger : ILogger
    {
        void Write<TData>(TData data);
    }
}