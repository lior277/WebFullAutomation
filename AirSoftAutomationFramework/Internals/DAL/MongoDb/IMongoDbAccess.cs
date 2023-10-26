// Ignore Spelling: Mongo

using MongoDB.Driver;
using System.Collections.Generic;
using AirSoftAutomationFramework.Objects.DTOs;

namespace AirSoftAutomationFramework.Internals.DAL.MongoDb
{
    public interface IMongoDbAccess
    {
        List<T> SelectAllDocumentsFromTable<T>(IMongoDatabase mongoDatabase, string tableName);
        AppSettings SelectConfigurationByBrandName(IMongoDatabase mongoDatabase, string tableName, string brandName);
        IMongoDbAccess UpdateDocument(IMongoDatabase mongoDatabase, string tableName, string filterName, string filterValue, string valueName, string valueForUpdate);
    }
}