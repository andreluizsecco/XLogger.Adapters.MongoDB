using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using XLogger.Adapters.MongoDB.Models;

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

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="data">The entry to be written. Can be also an object.</param>
        Task WriteAsync<TData>(TData data);
        
        /// <summary>
        /// Gets a fluent find interface of the custom model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A fluent find iterface of custom model log documents.</returns>
        IFindFluent<TDocument, TDocument> GetCustomLogs<TDocument>(Expression<Func<TDocument, bool>> filter = null, FindOptions options = null);

        /// <summary>
        /// Gets an async cursor of the custom model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">your custom document model.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>An asynchronous cursor of custom model log documents.</returns>
        Task<IAsyncCursor<TDocument>> GetCustomLogsAsync<TDocument>(Expression<Func<TDocument, bool>> filter = null, FindOptions<TDocument, TDocument> options = null);

        /// <summary>
        /// Gets a fluent find interface of the default model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">the data type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A fluent find iterface of default model log documents.</returns>
        IFindFluent<Log<TData>, Log<TData>> GetLogs<TData>(Expression<Func<Log<TData>, bool>> filter = null, FindOptions options = null);

        /// <summary>
        /// Gets an async cursor of the default model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">the data type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>An asynchronous cursor of default model log documents.</returns>
        Task<IAsyncCursor<Log<TData>>> GetLogsAsync<TData>(Expression<Func<Log<TData>, bool>> filter = null, FindOptions<Log<TData>, Log<TData>> options = null);
    }
}