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
        /// Gets the custom model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of custom model log documents.</returns>
        IEnumerable<TDocument> GetCustomLogs<TDocument>(Expression<Func<TDocument, bool>> filter = null, FindOptions options = null);

        /// <summary>
        /// Gets the custom model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of custom model log documents.</returns>
        Task<IEnumerable<TDocument>> GetCustomLogsAsync<TDocument>(Expression<Func<TDocument, bool>> filter = null, FindOptions<TDocument, TDocument> options = null);

        /// <summary>
        /// Gets the default model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of default model log documents.</returns>
        IEnumerable<Log<TData>> GetLogs<TData>(Expression<Func<Log<TData>, bool>> filter = null, FindOptions options = null);

        /// <summary>
        /// Gets the default model log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of default model log documents.</returns>
        Task<IEnumerable<Log<TData>>> GetLogsAsync<TData>(Expression<Func<Log<TData>, bool>> filter = null, FindOptions<Log<TData>, Log<TData>> options = null);
    }
}