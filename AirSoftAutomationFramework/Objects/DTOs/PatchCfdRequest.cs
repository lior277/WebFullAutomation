using System.Collections.Generic;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class PatchCfdRequest
    {
        public List<Asset> assets { get; set; }
    }

    public class Asset
    {
        public string _id { get; set; }
        public string label { get; set; }
        public string category { get; set; }
        public string sub_category { get; set; }
        public bool show_in_front { get; set; }
        public int order { get; set; }
        public string symbol { get; set; }
        public string exchange { get; set; }
    }
}
