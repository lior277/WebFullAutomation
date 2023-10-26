using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GroupRestrictionsGeneral
    {
        public string _id { get; set; }

        [JsonProperty("commodities")]
        public GroupRestrictionsCommodities groupRestrictionsCommodities { get; set; }

        [JsonProperty("cash")]
        public GroupRestrictionsCash groupRestrictionsCash { get; set; }

        [JsonProperty("default_attr")]
        public GroupRestrictionsDefaultAttr groupRestrictionsDefaultAttr { get; set; }

        [JsonProperty("forex")]
        public GroupRestrictionsForex groupRestrictionsForex { get; set; }

        [JsonProperty("indices")]
        public GroupRestrictionsIndices groupRestrictionsIndices { get; set; }

        [JsonProperty("stock")]
        public GroupRestrictionsStock groupRestrictionsStock { get; set; }

        [JsonProperty("cmdty")]
        public GroupRestrictionsCmdty groupRestrictionsCmdty { get; set; }

        [JsonProperty("stk")]
        public GroupRestrictionsStk groupRestrictionsStk { get; set; }

        [JsonProperty("ind")]
        public GroupRestrictionsInd groupRestrictionsInd { get; set; }

        [JsonProperty("crypto")]
        public GroupRestrictionsCrypto groupRestrictionsCrypto { get; set; }

        [JsonProperty("nft")]
        public GroupRestrictionsNft groupRestrictionsNft { get; set; }
    }

    public class GroupRestrictionsCommodities
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsNft
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsCash
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsDefaultAttr
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsForex
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsIndices
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsStock
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsCmdty
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsStk
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsInd
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }

    public class GroupRestrictionsCrypto
    {
        public int minus { get; set; }
        public int plus { get; set; }
    }
}
