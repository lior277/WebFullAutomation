// Ignore Spelling: Jira app Mongo

using AirSoftAutomationFramework.Internals.DAL.MongoDb;
using AirSoftAutomationFramework.Internals.Helpers;
using AirSoftAutomationFramework.Objects.DTOs;
using MongoDB.Driver;
using System;
using System.Linq;

namespace AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement
{
    public static class Config
    {
        public const string QaAuto01 = "qa-auto01";
        public const string Jira01 = "jira01";
        public const string QaDevAuto = "qa-dev-auto";
        public static AppSettings appSettings;

        public static string GetBrandName()
        {
            //### Choose System Type
            var localBrandName = Jira01;
           //###

            var brandNameFromMongo = Environment.GetEnvironmentVariable("BrandName");

            return brandNameFromMongo ?? localBrandName;
        }

        // populate AppSettings object from mongo site table
        public static void SetConfigurationFromMongoByBrandName(string actualBrandName)
        {
            var testimDbConnectionString = DataRep.TestimDbConnectionString;
            var mongoDbAccess = new MongoDbAccess();
            var mongoBrandClient = new MongoClient(testimDbConnectionString);
            var dbName = testimDbConnectionString.Split("net").Last().Trim('/');
            var mongoDatabase = mongoBrandClient.GetDatabase(dbName);

            mongoDbAccess.SelectConfigurationByBrandName(mongoDatabase,
                DataRep.SitesTable, actualBrandName);
        }

        public static void GetConfigurationFromMongoByBrandNamePipe()
        {
            var brandName = GetBrandName();
            SetConfigurationFromMongoByBrandName(brandName);
        }
    }
}
