using System;
using Microsoft.Extensions.Logging;
using XLogger.Options;

namespace XLogger.Adapters.MongoDB
{
    public class MongoDBLogger : IMongoDBLogger
    {
        /// <summary>
        /// The current logger options.
        /// </summary>
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

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        public void Write<TData>(TData data) =>
            _mongoDBContext.InsertOne(data);

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
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
                        exception.HResult,
                        exception.HelpLink,
                        InnerException = new
                        {
                            exception.InnerException?.HResult,
                            exception.InnerException?.HelpLink,
                            exception.InnerException?.Message,
                            exception.InnerException?.Source,
                            exception.InnerException?.StackTrace
                        },
                        exception.Message,
                        exception.Source,
                        exception.StackTrace
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

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a string message of the data and exception.</param>
        public void Write<TData>(LogLevel logLevel, EventId eventId, TData data, Exception exception, Func<TData, Exception, string> formatter)
        {
            if (formatter != null)
                Write(logLevel, formatter(data, exception), exception);
            else
                Write(logLevel, data, exception);
        }

        /// <summary>
        /// Writes a log entry on trace level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public void Trace<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Trace, data, exception, formatter);

        /// <summary>
        /// Writes a log entry on debug level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public void Debug<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Debug, data, exception, formatter);

        /// <summary>
        /// Writes a log entry on information level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public void Information<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Information, data, exception, formatter);

        /// <summary>
        /// Writes a log entry on warning level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public void Warning<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Warning, data, exception, formatter);

        /// <summary>
        /// Writes a log entry on error level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public void Error<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Error, data, exception, formatter);

        /// <summary>
        /// Writes a log entry on critical level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public void Critical<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Critical, data, exception, formatter);

        public void Dispose() =>
            _mongoDBContext = null;
    }
}