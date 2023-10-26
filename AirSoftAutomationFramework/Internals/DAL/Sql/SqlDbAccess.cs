// Ignore Spelling: Sql

using AirSoftAutomationFramework.Internals.DAL.ConfigurationManagement;

using MySqlConnector;

namespace AirSoftAutomationFramework.Internals.DAL.Sql
{
    public class SqlDbAccess : ISqlDbAccess
    {
        private string conString = Config.appSettings.SqlConnectionString;

        public MySqlDataReader RetrieveDataFromSqlDb(string command)
        {
            using var con = new MySqlConnection(conString);
            con.Open();
            using var cmd = new MySqlCommand(command, con);
            cmd.Connection = con;

            var rdr = cmd.ExecuteReader();

            //using (sda = new MySqlDataAdapter(cmd))
            //{
            //    using (DataTable dt = new DataTable())
            //    {                            
            //        sda.Fill(dt);
            //        if (dt.Rows.Count > 0)
            //        {
            //            for (var i = 0; i < dt.Rows.Count; i++)
            //            {
            //                JObject eachRowObj = new JObject();

            //                for (var j = 0; j < dt.Columns.Count; j++)
            //                {
            //                    data.Add(dt.Columns[j].ToString(), dt.Rows[i].ItemArray[j].ToString());                                      
            //                }
            //            }

            //            return data;
            //        }
            //        else
            //        {
            //            return null;
            //        }

            //}
            //}
            return rdr;
        }
    }

}
