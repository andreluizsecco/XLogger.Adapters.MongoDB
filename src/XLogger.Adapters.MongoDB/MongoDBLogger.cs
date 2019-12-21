using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XLogger.Adapters.MongoDB.Models;
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
        /// <param name="data">The entry to be written. Can be also an object.</param>
        public async Task WriteAsync<TData>(TData data) =>
            await _mongoDBContext.InsertOneAsync(data);

        private object CreateDefaultLogObject<TData>(DateTime datetime, LogLevel logLevel, TData data, Exception exception)
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
            return new
            {   
                DateTime = DateTime.Now,
                LogLevel = logLevel,
                Data = data,
                Exception = customException
            };
        }

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
                _mongoDBContext.InsertOne(formatter.Invoke(data, exception));
            else
            {
                var log = CreateDefaultLogObject(DateTime.Now, logLevel, data, exception);
                _mongoDBContext.InsertOne(log);
            }
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public async Task WriteAsync<TData>(LogLevel logLevel, TData data, Exception exception = null, Func<TData, Exception, object> formatter = null)
        {
            if (formatter != null)
                await _mongoDBContext.InsertOneAsync(formatter.Invoke(data, exception));
            else
            {
                var log = CreateDefaultLogObject(DateTime.Now, logLevel, data, exception);
                await _mongoDBContext.InsertOneAsync(log);
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
        /// Writes a log entry on trace level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public async Task TraceAsync<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            await WriteAsync(LogLevel.Trace, data, exception, formatter);

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
        /// Writes a log entry on debug level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public async Task DebugAsync<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            await WriteAsync(LogLevel.Debug, data, exception, formatter);

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
        /// Writes a log entry on information level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public async Task InformationAsync<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            await WriteAsync(LogLevel.Information, data, exception, formatter);

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
        /// Writes a log entry on warning level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public async Task WarningAsync<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            await WriteAsync(LogLevel.Warning, data, exception, formatter);

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
        /// Writes a log entry on error level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public async Task ErrorAsync<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            await WriteAsync(LogLevel.Error, data, exception, formatter);

        /// <summary>
        /// Writes a log entry on critical level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public void Critical<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            Write(LogLevel.Critical, data, exception, formatter);

        /// <summary>
        /// Writes a log entry on critical level.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a custom object of the data and exception.</param>
        public async Task CriticalAsync<TData>(TData data, Exception exception = null, Func<TData, Exception, object> formatter = null) =>
            await WriteAsync(LogLevel.Critical, data, exception, formatter);

        /// <summary>
        /// Gets a fluent find interface of the custom model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">your custom document model.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A fluent find iterface of custom model log documents.</returns>
        public IFindFluent<TDocument, TDocument> GetCustomLogs<TDocument>(Expression<Func<TDocument, bool>> filter = null, FindOptions options = null) =>
            _mongoDBContext.Get(filter, options);

        /// <summary>
        /// Gets an async cursor of the custom model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">your custom document model.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>An asynchronous cursor of custom model log documents.</returns>
        public async Task<IAsyncCursor<TDocument>> GetCustomLogsAsync<TDocument>(Expression<Func<TDocument, bool>> filter = null, FindOptions<TDocument, TDocument> options = null) =>
            await _mongoDBContext.GetAsync(filter, options);

        /// <summary>
        /// Gets a fluent find interface of the default model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">the data type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A fluent find iterface of default model log documents.</returns>
        public IFindFluent<Log<TData>, Log<TData>> GetLogs<TData>(Expression<Func<Log<TData>, bool>> filter = null, FindOptions options = null) =>
            _mongoDBContext.Get(filter, options);

        /// <summary>
        /// Gets an async cursor of the default model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">the data type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>An asynchronous cursor of default model log documents.</returns>
        public async Task<IAsyncCursor<Log<TData>>> GetLogsAsync<TData>(Expression<Func<Log<TData>, bool>> filter = null, FindOptions<Log<TData>, Log<TData>> options = null) =>
            await _mongoDBContext.GetAsync(filter, options);

        public void Dispose() =>
            _mongoDBContext = null;
    }
}