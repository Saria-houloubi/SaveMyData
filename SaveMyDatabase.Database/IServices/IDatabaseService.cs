using MongoDB.Driver;
using SaveMyDataServer.SharedKernal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaveMyDataServer.Database.IServices
{
    /// <summary>
    /// The service interface to work with CRUD opperations of the database 
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Adds a record into the database
        /// </summary>
        /// <typeparam name="T">A generic type of the object to add</typeparam>
        /// <param name="record">The record to add</param>
        /// <param name="table">The table or documant to add the record into</param>
        /// <param name="database">The database we are working with</param>
        /// <returns></returns>
        Task<T> AddRecord<T>(T record, string table);
        /// <summary>
        /// Updates a record in the database
        /// </summary>
        /// <typeparam name="T">The type of record that we want to update</typeparam>
        /// <param name="record">The record to update</param>
        /// <param name="id">The unique if od the record</param>
        /// <param name="table">The table the record is saved in</param>
        /// <returns></returns>
        Task<T> ReplaceRecordById<T>(T record,string id, string table);
        /// <summary>
        /// Gets a record from the database depending on the sent filters values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The name in the table</param>
        /// <param name="value">The value to compare it to</param>
        /// <param name="table">The table that will search in</param>
        /// <param name="database">The database the user is using</param>
        /// <returns></returns>
        Task<T> GetRecord<T>(string field, object value, string table);
        /// <summary>
        /// Gets a list of records from the wanted collection
        /// </summary>
        /// <typeparam name="T">The type of of records</typeparam>
        /// <param name="table">The table name of records</param>
        /// <param name="filters">Any filters to preform on the "SELECT" statmetn</param>
        /// <param name="pagination">The pagination information to skip and take</param>
        /// <returns></returns>
        Task<List<T>> GetRecords<T>(string table, FilterDefinition<T> filters = null, PaginationModel pagination = null);
        /// <summary>
        /// Gets the table names and row count that for each table in the connected database
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, long>> GetTables();
        /// <summary>
        /// Delete a record from the database
        /// </summary>
        /// <typeparam name="T">The type of record to delete</typeparam>
        /// <param name="id">The id of the record in the database</param>
        /// <param name="table">The table to find the record in</param>
        /// <returns></returns>
        Task<T> DeleteRecord<T>(string id, string table);


        /// <summary>
        /// Initilizes which database to start connecting and using
        /// </summary>
        /// <param name="databaseName">The database name the user created</param>
        /// <param name="create">If ture will create the database</param>
        void InitilizeDatabase(string databaseName,bool create =false);

    }
}
