using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class MessagesMongoTable
    {

        public _Id _id { get; set; }
        public string type { get; set; }
        public Created_At created_at { get; set; }
        public string created_by { get; set; }
        public Broadcast_Date broadcast_date { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("users")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public MongoUser[] users { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("message")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public MongoMessage message { get; set; }
        public string status { get; set; }
        public Read_By[] read_by { get; set; }
        public Last_Update last_update { get; set; }
    }

    public class _Id
    {
        public string oid { get; set; }
    }

    public class Created_At
    {
        public DateTime date { get; set; }
    }

    public class Broadcast_Date
    {
        public DateTime date { get; set; }
    }

    public class MongoMessage
    {
        public string subject { get; set; }
        public string body { get; set; }
    }

    public class Last_Update
    {
        public DateTime date { get; set; }
    }


    public class MongoUser
    {
        public string oid { get; set; }
    }

    public class Read_By
    {
        public string oid { get; set; }
    }
}
