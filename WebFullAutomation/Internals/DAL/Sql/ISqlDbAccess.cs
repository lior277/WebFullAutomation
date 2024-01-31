// Ignore Spelling: Sql

using MySqlConnector;

namespace AirSoftAutomationFramework.Internals.DAL.Sql
{
    public interface ISqlDbAccess
    {
        MySqlDataReader RetrieveDataFromSqlDb(string command);
    }
}