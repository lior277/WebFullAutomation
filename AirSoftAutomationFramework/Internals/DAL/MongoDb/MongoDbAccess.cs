// Ignore Spelling: Mongo

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;
using AirSoftAutomationFramework.Internals.ExtensionsMethods;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Internals.DAL.MongoDb
{
    public class MongoDbAccess : IMongoDbAccess
    {
        public List<T> SelectAllDocumentsFromTable<T>(IMongoDatabase mongoDatabase,
            string tableName)
        {
            var searchResultBsonDocument = new List<BsonDocument>();
            var listOfT = new List<T>();

            try
            {
                var listString = new List<string>();
                var listObjects = new List<object>();

                for (var i = 0; i < 4; i++)
                {
                    var toNetOblects = mongoDatabase
                        .GetCollection<BsonDocument>(tableName)
                        .Find(new BsonDocument());

                    if (toNetOblects == null)
                    {
                        Thread.Sleep(300);
                    }
                    else
                    {
                        //var timeoutCts = new CancellationTokenSource();

                        try
                        {
                            //timeoutCts.CancelAfter(TimeSpan.FromSeconds(30)); 

                            foreach (var item in toNetOblects.ToList())
                            {
                                listObjects.Add(BsonTypeMapper.MapToDotNetValue(item));
                            }
                        }
                        catch (OperationCanceledException ex)
                        {
                            var exceMessage = ($" Exception Message: {ex?.Message}");
                            var exception = new OperationCanceledException(exceMessage);

                            throw exception;
                        }
                        
                        break;
                            //.ForEach(o => listObjects.Add(BsonTypeMapper.MapToDotNetValue(o)));
                            //ConvertAll(BsonTypeMapper.MapToDotNetValue);
                    }
                }

                listObjects.ForEach(x => listString.Add(JsonConvert.SerializeObject(x)));

                listString
                      .ForEach(p => listOfT.Add(JsonConvert.DeserializeObject<T>(p)));

                return listOfT;
            }
            catch (Exception ex)
            {
                var exceMessage = $"search Result Bson Document: {listOfT.ListToString()}," +
                    $" exception: {ex.Message}";

                var exception = new Exception(exceMessage);

                throw exception;
            }
        }


        public AppSettings SelectConfigurationByBrandName(IMongoDatabase mongoDatabase,
           string tableName, string brandName)
        {
            var searchResultBsonDocument = new List<BsonDocument>();
            var listOfT = new List<AppSettings>();

            try
            {
                var listString = new List<string>();

                var toNetOblects = mongoDatabase
                    .GetCollection<BsonDocument>(tableName)
                    .Find(new BsonDocument())
                    //.Project(projection)
                    .ToList()
                    .ConvertAll(BsonTypeMapper.MapToDotNetValue);

                toNetOblects.ForEach(x => listString.Add(JsonConvert.SerializeObject(x)));

                var configurationJson = listString
                    .Where(p => p.Contains(brandName))
                    .FirstOrDefault();

                return Config.appSettings =  JsonConvert.DeserializeObject<AppSettings>(configurationJson);
            }
            catch (Exception ex)
            {
                var exceMessage = $"search Result Bson Document: {listOfT.ListToString()}," +
                    $" exception: {ex?.Message}";

                var exception = new Exception(exceMessage);

                throw exception;
            }
        }

        public IMongoDbAccess UpdateDocument(IMongoDatabase mongoDatabase, string tableName,
            string filterName, string filterValue, string valueName, string valueForUpdate)
        {
            var collection = mongoDatabase.GetCollection<BsonDocument>(tableName);
            var filter = Builders<BsonDocument>.Filter.Eq(filterName, filterValue);
            var update = Builders<BsonDocument>.Update.Set(valueName, valueForUpdate);
            collection.UpdateOne(filter, update);

            return this;
        }

    }
}
