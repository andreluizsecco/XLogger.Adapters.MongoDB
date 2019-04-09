using System;
using Microsoft.Extensions.Logging;
using XLogger.Options;

namespace XLogger.Adapters.MongoDB
{
    public class MongoDBLogger : IMongoDBLogger
    {
        public ILoggerOptions Options { get; }
        private MongoDBContext _mongoDBContext;

        public MongoDBLogger()
        {
            Options = new MongoDBLoggerOptions();
            CreateMongoDBContext(Options);
        }

        public MongoDBLogger(MongoDBLoggerOptions options)
        {
            Options = options;
            CreateMongoDBContext(Options);
        }

        public MongoDBLogger(Action<MongoDBLoggerOptions> options)
        {
            Options = new MongoDBLoggerOptions();
            options.Invoke((MongoDBLoggerOptions)Options);
            CreateMongoDBContext(Options);
        }

        private MongoDBContext CreateMongoDBContext(ILoggerOptions options) =>
            _mongoDBContext = new MongoDBContext((MongoDBLoggerOptions)options);

        public IDisposable BeginScope<TData>(TData data) => null;

        public void Write<TData>(TData data) =>
            _mongoDBContext.InsertOne(data);

        public void Write<TData>(LogLevel logLevel, TData data, Exception exception = null, Func<TData, Exception, object> formatter = null)
        {
            if (formatter != null)
                _mongoDBContext.InsertOne<object>(formatter.Invoke(data, exception));
            else
            {
                object customException = null;
                if (exception != null)
                    customException = new 
                    {
                        HResult = exception.HResult,
                        HelpLink = exception.HelpLink,
                        InnerException = new
                        {
                            HResult = exception.InnerException?.HResult,
                            HelpLink = exception.InnerException?.HelpLink,
                            Message = exception.InnerException?.Message,
                            Source = exception.InnerException?.Source,
                            StackTrace = exception.InnerException?.StackTrace
                        },
                        Message = exception.Message,
                        Source = exception.Source,
                        StackTrace = exception.StackTrace
                    };

                var log = new 
                {
                    DateTime = DateTime.Now,
                    LogLevel = logLevel.ToString(),
                    Data = data,
                    Exception = customException
                };
                _mongoDBContext.InsertOne<object>(log);
            }
        }

        public void Write<TData>(LogLevel logLevel, EventId eventId, TData data, Exception exception, Func<TData, Exception, string> formatter)
        {
            if (formatter != null)
                Write(logLevel, formatter(data, exception), exception);
            else
                Write(logLevel, data, exception);
        }

        public void Trace<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Trace, data, exception, formatter);

        public void Debug<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Debug, data, exception, formatter);

        public void Information<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Information, data, exception, formatter);

        public void Warning<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Warning, data, exception, formatter);

        public void Error<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Error, data, exception, formatter);

        public void Critical<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Critical, data, exception, formatter);

        public void Dispose() =>
            _mongoDBContext = null;
    }
}