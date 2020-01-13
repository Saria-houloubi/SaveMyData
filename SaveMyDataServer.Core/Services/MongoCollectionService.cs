﻿using MongoDB.Driver;
using SaveMyDataServer.Core.IServices;
using SaveMyDataServer.Database.IServices;
using SaveMyDataServer.SharedKernal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaveMyDataServer.Core.Services
{
    /// <summary>
    /// The implmentation and the logic to work with collections
    /// </summary>
    public class CollectionService : IMongoCollectionService
    {

        #region Services
        /// <summary>
        /// The database acesss layer
        /// </summary>
        public IDatabaseService DatabaseService { get; private set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public CollectionService(IDatabaseService databaseService)
        {
            DatabaseService = databaseService;
        }
        #endregion

        public async Task<T> AddRecord<T>(T record, string table, string database)
        {
            //Connect to the wanted database
            DatabaseService.InitilizeDatabase(database, true);
            //Add the record into the database
            var dbRecord = await DatabaseService.AddRecord<T>(record, table);
            //Return the added record to the user
            return dbRecord;
        }
        public async Task<Dictionary<string, long>> GetDatabaseTables(string database)
        {
            //Connect to the wanted database
            DatabaseService.InitilizeDatabase(database);

            var tables = await DatabaseService.GetTables();

            return tables;
        }
        public async Task<List<T>> GetCollectionFilterd<T>(string table, string database, FilterDefinition<T> filterDefinition = null, PaginationModel pagination = null)
        {
            //Connect to the wanted database
            DatabaseService.InitilizeDatabase(database);

            return await DatabaseService.GetRecords<T>(table, filterDefinition, pagination);
        }
        public async Task<List<T>> GetCollection<T>(string table, string database, PaginationModel pagination = null)
        {
            //Connect to the wanted database
            DatabaseService.InitilizeDatabase(database);

            return await DatabaseService.GetRecords<T>(table, pagination: pagination);
        }
        public async Task<T> DeleteRecordById<T>(string id, string table, string database)
        {
            //Connect tot thw wanted database
            DatabaseService.InitilizeDatabase(database);

            return await DatabaseService.DeleteRecord<T>(id, table);

        }
        public async Task<T> EditHoleRecordById<T>(T record, string id, string table, string database)
        {
            //Connect tot thw wanted database
            DatabaseService.InitilizeDatabase(database);

            return await DatabaseService.ReplaceRecordById<T>(record, id, table);
        }
    }
}