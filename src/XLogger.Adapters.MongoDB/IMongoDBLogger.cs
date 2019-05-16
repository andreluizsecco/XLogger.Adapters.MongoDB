using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using XLogger.Models;

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
        /// Gets the default log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of log documents.</returns>
        IEnumerable<Log<TData>> Get<TData>(Expression<Func<Log<TData>, bool>> filter, FindOptions options = null);

        /// <summary>
        /// Gets the default log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TData">type of entry.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of log documents.</returns>
        Task<IEnumerable<Log<TData>>> GetAsync<TData>(Expression<Func<Log<TData>, bool>> filter, FindOptions<Log<TData>, Log<TData>> options = null);

        /// <summary>
        /// Gets the custom log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of custom log documents.</returns>
        IEnumerable<TDocument> Get<TDocument>(Expression<Func<TDocument, bool>> filter, FindOptions options = null);

        /// <summary>
        /// Gets the custom log documents based on filter and the find options.
        /// </summary>
        /// <typeparam name="TDocument">the document type.</typeparam>
        /// <param name="filter">filter expression.</param>
        /// <param name="options">options for finding documents.</param>
        /// <returns>A list of custom log documents.</returns>
        Task<IEnumerable<TDocument>> GetAsync<TDocument>(Expression<Func<TDocument, bool>> filter, FindOptions<TDocument, TDocument> options = null);
    }
}