// Ignore Spelling: Mongo Crm

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using MongoDB.Driver;
using System;

namespace AirSoftAutomationFramework.Internals.DAL.MongoDb
{
    public sealed class InitializeMongoClient
    {
        private static IMongoClient _mongoBrandClient;
        private static IMongoClient _mongoAutomationDataClient;
        private static IMongoDatabase _mongoBrandDatabase;
        private static IMongoDatabase _mongoAutomationDataDatabase;
        private static object _syncLock = new object();
        private static string _mongoDbConnectionString;
        private static string _mongoAutomationDataConnectionString;

        private InitializeMongoClient() { }

        static InitializeMongoClient()
        {
            _mongoDbConnectionString = Config.appSettings.MongoConnectionString;
            _mongoAutomationDataConnectionString = Config.appSettings.AutomationDbConnectionString;
        }

        public static IMongoDatabase ConnectToCrmMongoDb
        {
            get
            {
                lock (_syncLock)
                {
                    if (_mongoBrandClient == null)
                    {
                        //var mongoClientSettings = MongoClientSettings.FromConnectionString(_mongoCrmConnectionString);
                        //mongoClientSettings.ConnectTimeout = new TimeSpan(0, 0, 0, 2);
                        _mongoBrandClient = new MongoClient(_mongoDbConnectionString);
                        var dbName = _mongoDbConnectionString.Split("net")[1].Trim('/');
                        _mongoBrandDatabase = _mongoBrandClient.GetDatabase(dbName);
                    }
                }

                return _mongoBrandDatabase;
            }
        }      

        public static IMongoDatabase ConnectToAutomationMongoDb
        {
            get
            {
                string dbName = null;

                lock (_syncLock)
                {
                    if (_mongoAutomationDataClient == null)
                    {
                        var mongoClientSettings = MongoClientSettings.FromConnectionString(_mongoAutomationDataConnectionString);
                        mongoClientSettings.ConnectTimeout = TimeSpan.FromSeconds(1);
                        _mongoAutomationDataClient = new MongoClient(mongoClientSettings);
                        dbName = _mongoAutomationDataConnectionString.Split("net")[1].Trim('/');
                        _mongoAutomationDataDatabase = _mongoAutomationDataClient.GetDatabase(dbName);
                    }
                }

                return _mongoAutomationDataDatabase;
            }
        }       
    }
}
