namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PatchKycFileResponse
    {
        public Result result { get; set; }
        //public Fields fields { get; set; }
    }

    public class Result
    {
        public Result1 result { get; set; }
        public Connection connection { get; set; }
        public int modifiedCount { get; set; }
        public object upsertedId { get; set; }
        public int upsertedCount { get; set; }
        public int matchedCount { get; set; }
        public int n { get; set; }
        public int nModified { get; set; }
        public Optime1 opTime { get; set; }
        public string electionId { get; set; }
        public int ok { get; set; }
        public string operationTime { get; set; }
        public Clustertime1 clusterTime { get; set; }
    }

    public class Result1
    {
        public int n { get; set; }
        public int nModified { get; set; }
        public Optime opTime { get; set; }
        public string electionId { get; set; }
        public int ok { get; set; }
        public string operationTime { get; set; }
        public Clustertime clusterTime { get; set; }
    }

    public class Optime
    {
        public string ts { get; set; }
        public int t { get; set; }
    }

    public class Clustertime
    {
        public string clusterTime { get; set; }
        public Signature signature { get; set; }
    }

    public class Signature
    {
        public string hash { get; set; }
        public string keyId { get; set; }
    }

    public class Connection
    {
        public _Events _events { get; set; }
        public int _eventsCount { get; set; }
        public int id { get; set; }
        public string address { get; set; }
        public Bson bson { get; set; }
        public int socketTimeout { get; set; }
        public string host { get; set; }
        public int port { get; set; }
        public bool monitorCommands { get; set; }
        public bool closed { get; set; }
        public bool destroyed { get; set; }
        public int lastIsMasterMS { get; set; }
    }

    public class _Events
    {
    }

    public class Bson
    {
    }

    public class Optime1
    {
        public string ts { get; set; }
        public int t { get; set; }
    }

    public class Clustertime1
    {
        public string clusterTime { get; set; }
        public Signature1 signature { get; set; }
    }

    public class Signature1
    {
        public string hash { get; set; }
        public string keyId { get; set; }
    }

    //public class Fields
    //{
    //    public string kyc_proof_of_identity { get; set; }
    //    public string kyc_proof_of_identity_status { get; set; }
    //    public DateTime last_update { get; set; }
    //}
}
