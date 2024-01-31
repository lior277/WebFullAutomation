namespace AirSoftAutomationFramework.Objects.DTOs
{
#pragma warning disable RCS1102 // Make class static.
    public class GetCfdResponse
#pragma warning restore RCS1102 // Make class static.
    {
        public class AssetData
        {
            public string _id { get; set; }
            public string symbol { get; set; }
            public string label { get; set; }
            public Cfd cfd { get; set; }
            public string exchange { get; set; }
            public string category { get; set; }
        }

        public class Cfd
        {
            public bool include { get; set; }
            public bool show_in_front { get; set; }
            public int order { get; set; }
            public string category { get; set; }
            public string sub_category { get; set; }
        }

    }
}

