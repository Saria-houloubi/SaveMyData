using MongoDB.Bson;
using MongoDB.Driver;
using SaveMyDataServer.Database.IServices;
using SaveMyDataServer.Database.Models.Users;
using SaveMyDataServer.Database.Static;
using SaveMyDataServer.SharedKernal;
using SaveMyDataServer.SharedKernal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SaveMyDataServer.Database.Services
{
    /// <summary>
    /// The implmentation for the service layer
    /// </summary>
    public class MongoDatabaseService : IDatabaseService
    {

        #region Properties
        /// <summary>
        /// The mongodatbase to do operrations on
        /// </summary>
        public IMongoDatabase MongoDatabase { get; private set; }
        #endregion

        #region Constructer
        /// <summary>
        /// Default constructer
        /// </summary>
        public MongoDatabaseService()
        {

        }
        #endregion
        public async Task<T> AddRecord<T>(T record, string table)
        {

            //Get the collection from the databsae
            var collection = MongoDatabase.GetCollection<T>(table);

            //Wait until the record is added
            await collection.InsertOneAsync(record);

            return record;
        }

        public async Task<T> ReplaceRecordById<T>(T record, string id, string table)
        {
            //If the record is a bson document
            if (record.GetType() == typeof(BsonDocument))
            {
                //Get the collection from the databsae
                var collection = MongoDatabase.GetCollection<BsonDocument>(table);
                //Cast the record to a bson document
                var recordAsBsonDoc = (record as BsonDocument);
                //Get the id element from the record
                var idElement = recordAsBsonDoc.Elements.SingleOrDefault(item => item.Name == "Id");
                //remove the id element for replacement
                recordAsBsonDoc.RemoveElement(idElement);
                //Select the document by the id filed
                var filter = Builders<BsonDocument>.Filter.Eq(MongoTableBaseFieldNames.Id, new ObjectId(idElement.Value.AsString));
                //Update the document in the database
                await collection.FindOneAndReplaceAsync<BsonDocument>(filter, recordAsBsonDoc);
            }
            else
            {
                var collection = MongoDatabase.GetCollection<T>(table);
                //Select the document by the id filed
                var filter = Builders<T>.Filter.Eq(MongoTableBaseFieldNames.Id, ((dynamic)record).Id);
                //Update the document in the database
                await collection.ReplaceOneAsync(filter, record);
            }

            return record;
        }
        public async Task<T> GetRecord<T>(string field, object value, string table)
        {
            //Get the collection from the database
            var collection = MongoDatabase.GetCollection<T>(table);
            //Create the builder object
            var builder = Builders<T>.Filter;
            //Create the filter
            var filter = builder.Eq(field, value);
            //Get the object from the database
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<List<T>> GetRecords<T>(string table, FilterDefinition<T> filters = null, PaginationModel pagination = null)
        {
            //Get the collection from the database
            var collection = MongoDatabase.GetCollection<T>(table);
            //Get the records in the collection
            var data = await collection.Find(filters ?? new BsonDocument()).ToListAsync<T>();
            //If the count of data that we want is given then only get that amount
            return pagination == null ? data : data.Skip((pagination.CurrentPage - 1) * pagination.FetchRecordCount).Take(pagination.FetchRecordCount).ToList();
        }
        public async Task<long> GetRecordsCount<T>(string table, Expression<Func<T, bool>> filter)
        {
            //Get the collection from the database
            return await MongoDatabase.GetCollection<T>(table).CountDocumentsAsync<T>(filter);
        }


        public async Task<Dictionary<string, long>> GetTables()
        {
            //Create the dictonary to hold the collections and there documnet count
            var collectionAndCount = new Dictionary<string, long>();

            var collections = MongoDatabase.ListCollections().ToList();

            foreach (var item in collections)
            {
                //Get the collection name and document count
                collectionAndCount.Add(item[0].ToString(), await MongoDatabase.GetCollection<BsonDocument>(item[0].ToString()).CountDocumentsAsync(new BsonDocument()));
            }
            return collectionAndCount;
        }
        public async Task<T> DeleteRecord<T>(Expression<Func<T, bool>> filter, string table)
        {
            //Get the collection from the database
            var collection = MongoDatabase.GetCollection<T>(table);

            //Get the item from the database and delete it
            var itemToDelete = await collection.FindOneAndDeleteAsync(filter);

            return itemToDelete;
        }

        public async Task<T> DeleteRecord<T>(FilterDefinition<T> filter, string table)
        {
            //Get the collection from the database
            var collection = MongoDatabase.GetCollection<T>(table);

            //Get the item from the database and delete it
            var itemToDelete = await collection.FindOneAndDeleteAsync(filter);

            return itemToDelete;
        }
        public async Task DropDatabase(string dbName)
        {
            // wait until the database is droped
            await MongoDatabase.Client.DropDatabaseAsync(dbName);
        }

        public async Task DropColleciton(string name)
        {
            // wait until the collection is droped
            await MongoDatabase.DropCollectionAsync(name);
        }
        public async Task RenameColleciton(string oldName, string newName)
        {
            // wait until the collection is droped
            await MongoDatabase.RenameCollectionAsync(oldName, newName);
        }

        public void InitilizeDatabase(string databaseName, bool create = false)
        {
            //Create the clinet
            var client = new MongoClient();
            //Open the connection to the database
            MongoDatabase = client.GetDatabase(databaseName);
            //If it is our main database then set some key indexes
            if (CheckIfCreated(databaseName, client))
            {
                //Check if the database was already created
                if (databaseName == DatabaseNames.Main)
                {
                    //Get the collection
                    var userCollection = MongoDatabase.GetCollection<UserModel>(DatabaseTableNames.Users);
                    var options = new CreateIndexOptions() { Unique = true };
                    var field = new StringFieldDefinition<UserModel>(MongoTableBaseFieldNames.Email);
                    var indexDef = new IndexKeysDefinitionBuilder<UserModel>().Ascending(field);
                    //Sets the email address as unique
                    userCollection.Indexes.CreateOne(new CreateIndexModel<UserModel>(indexDef, options));
                }
                else
                {
                    if (create)
                    {
                        //Connect to the main database
                        var mongoMainDatabase = client.GetDatabase(DatabaseNames.Main);
                        //Get the user database collection
                        var collection = mongoMainDatabase.GetCollection<UserDatabaseModel>(DatabaseTableNames.UserDatabases);
                        //Add the item to the collection
                        collection.InsertOne(new UserDatabaseModel
                        {
                            CreationDateUTC = DateTime.UtcNow,
                            FullName = databaseName,
                            Key = Guid.NewGuid(),
                            Name = databaseName.Split('_')[1],
                            UserId = Guid.Parse(databaseName.Split('_')[0])
                        });
                    }

                }
            }

        }

        #region Methods
        /// <summary>
        /// Checks if the database is already created or not
        /// </summary>
        /// <param name="dbName">The database name to search for</param>
        /// <param name="client">The MongoClient to search in</param>
        /// <returns></returns>
        private bool CheckIfCreated(string dbName, MongoClient client)
        {
            return client.ListDatabaseNames().ToList().Where(db => dbName == db).SingleOrDefault() == null;
        }

        #endregion

    }
}
