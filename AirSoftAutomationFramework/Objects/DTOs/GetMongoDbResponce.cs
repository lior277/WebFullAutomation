namespace AirSoftAutomationFramework.Objects.DTOs
{

    public class GetMongoDbResponce
    {
        public string Name { get; set; }
        public Value[] Value { get; set; }
    }

    public class Value
    {
        public string fingerprint { get; set; }
        public Expires expires { get; set; }
        public string signature { get; set; }
    }

    public class Expires
    {
        public long date { get; set; }
    }
}
