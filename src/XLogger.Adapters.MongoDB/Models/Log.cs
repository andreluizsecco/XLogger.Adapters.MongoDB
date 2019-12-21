using MongoDB.Bson;

namespace XLogger.Adapters.MongoDB.Models
{
    public class Log<TData> : XLogger.Models.Log<TData>
    {
        public ObjectId Id { get; private set; }
    }
}