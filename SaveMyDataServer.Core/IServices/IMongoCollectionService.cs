using MongoDB.Bson;
using MongoDB.Driver;
using SaveMyDataServer.SharedKernal.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SaveMyDataServer.Core.IServices
{
    /// <summary>
    /// The methods that we can do on a collection
    /// </summary>
    public interface IMongoCollectionService
    {
        /// <summary>
        /// Adds a record to the sent collection name
        /// </summary>
        /// <typeparam name="T">The type of the record to add</typeparam>
        /// <param name="record">The record data to add</param>
        /// <param name="table">The collection name to get and add the record to</param>
        /// <param name="database">The database to add the collection</param>
        /// <returns></returns>
        Task<T> AddRecord<T>(T record, string table, string database);
        /// <summary>
        /// Updates a record that matches the sent filter
        /// </summary>
        /// <param name="filters">The filters to match records by</param>
        /// <param name="update">The update query to change the matched records</param>
        /// <param name="table">The table to update in</param>
        /// <param name="database">The database to connect to</param>
        /// <returns></returns>
        Task<UpdateResult> UpdateReocrds(FilterDefinition<BsonDocument> filters, UpdateDefinition<BsonDocument> update, string table, string database);
        /// <summary>
        /// Edits all the properties of the record by replacing everthing in it
        /// </summary>
        /// <typeparam name="T">The type of record to update</typeparam>
        /// <param name="record">The new record data</param>
        /// <param name="table">The table the record is in</param>
        /// <param name="database">The database the user is connected to</param>
        /// <returns></returns>
        Task<T> EditHoleRecordById<T>(T record, string id, string table, string database);
        /// <summary>
        /// Renames a collection 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        Task RenameCollection(string oldName, string newName, string database);
        /// <summary>
        /// Gets the list of all the records that are in the collection
        /// </summary>
        /// <typeparam name="T">The type of the records to get</typeparam>
        /// <param name="table">The name of the table to get</param>
        /// <param name="database">the database the user is working on</param>
        /// <param name="pagination">The pagination information to skip and take</param>
        /// <returns></returns>
        Task<List<T>> GetCollection<T>(string table, string database, PaginationModel pagination = null);
        /// <summary>
        /// Gets the count of records in a table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="database"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<long> GetCollectionCount<T>(string table, string database, Expression<Func<T, bool>> filter);
        /// <summary>
        /// Overloaded method for getCollection with filtering abilties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <param name="database"></param>
        /// <param name="filterDefinition"></param>
        /// <param name="count">The number of records to get from the database</param>
        /// <param name="pagination">The pagination information to skip and take</param>
        /// <returns></returns>
        Task<List<T>> GetCollectionFilterd<T>(string table, string database, FilterDefinition<T> filterDefinition = null, PaginationModel pagination = null);
        /// <summary>
        /// Gets the table names and row count for the sent database
        /// </summary>
        /// <param name="database">Than naem of the database to get the data for</param>
        /// <returns></returns>
        Task<Dictionary<string, long>> GetDatabaseTables(string database);

        /// <summary>
        /// Delete a record in the database holding the sent id
        /// </summary>
        /// <typeparam name="T">The type of record to delete</typeparam>
        /// <param name="id">The id of the record to delete</param>
        /// <param name="table">The name of the table that holds the record</param>
        /// <param name="database">The database the user is working on</param>
        /// <returns></returns>
        Task<DeleteResult> DeleteRecordById<T>(string id, string table, string database);
        /// <summary>
        /// Deletes a record that matches the sent filetr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <param name="table"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        Task<T> DeleteRecord<T>(Expression<Func<T, bool>> filter, string table, string database);
        Task<DeleteResult> DeleteRecord<T>(FilterDefinition<T> filter, string table, string database);
        /// <summary>
        /// Drops a database only if empty 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        Task DropDatabase(string database);
        /// <summary>
        /// Drops a collection with the sent name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task DropCollection(string name, string database);
    }
}
