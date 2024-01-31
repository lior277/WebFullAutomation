// Ignore Spelling: stk crypto indices commision Cmdty forex

using System;
using Newtonsoft.Json;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class GetCryptoGroupsResponse
    {
        public string _id { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("assets")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupAssets assets { get; set; }
        public string name { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("default_attr")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupDefault_Attr default_attr { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("stk")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupStk stk { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("cash")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupCash cash { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("cmdty")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupCmdty cmdty { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("ind")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupInd ind { get; set; }
        public bool? _default { get; set; }
        public DateTime created_at { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("forex")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroup forex { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("commodities")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupCommodities commodities { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("indices")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupIndices indices { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("stock")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupStock stock { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("crypto")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupCrypto crypto { get; set; }

#pragma warning disable CA1507 // Use nameof to express symbol names
        [JsonProperty("futures")]
#pragma warning restore CA1507 // Use nameof to express symbol names
        public GetCryptoGroupFutures futures { get; set; }
        public DateTime? last_update { get; set; }
        public int revision { get; set; }
    }

    public class GetCryptoGroupAssets
    {
    }

    public class GetCryptoGroupDefault_Attr
    {
        public double commision { get; set; }
        public int leverage { get; set; }
        public double maintenance { get; set; }
        public int minimum_amount { get; set; }
        public int minimum_step { get; set; }
        public object margin_call { get; set; }
        public double spread { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public string swap_time { get; set; }
    }

    public class GetCryptoGroupStk
    {
        public object commision { get; set; }
        public int? leverage { get; set; }
        public double? maintenance { get; set; }
        public object minimum_amount { get; set; }
        public object minimum_step { get; set; }
        public double? spread { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupCash
    {
        public object commision { get; set; }
        public int? leverage { get; set; }
        public double? maintenance { get; set; }
        public object minimum_amount { get; set; }
        public object minimum_step { get; set; }
        public double? spread { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupCmdty
    {
        public object commision { get; set; }
        public int? leverage { get; set; }
        public double? maintenance { get; set; }
        public object minimum_amount { get; set; }
        public object minimum_step { get; set; }
        public double? spread { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupInd
    {
        public object commision { get; set; }
        public int? leverage { get; set; }
        public double? maintenance { get; set; }
        public object minimum_amount { get; set; }
        public object minimum_step { get; set; }
        public double? spread { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroup
    {
        public object commision { get; set; }
        public object swap_time { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object spread { get; set; }
        public object minimum_step { get; set; }
        public object minimum_amount { get; set; }
        public object leverage { get; set; }
        public object maintenance { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupCommodities
    {
        public object commision { get; set; }
        public object swap_time { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object spread { get; set; }
        public object minimum_step { get; set; }
        public object minimum_amount { get; set; }
        public object leverage { get; set; }
        public object maintenance { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupIndices
    {
        public object commision { get; set; }
        public object swap_time { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object spread { get; set; }
        public object minimum_step { get; set; }
        public object minimum_amount { get; set; }
        public object leverage { get; set; }
        public object maintenance { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupStock
    {
        public object commision { get; set; }
        public object swap_time { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object spread { get; set; }
        public object minimum_step { get; set; }
        public object minimum_amount { get; set; }
        public object leverage { get; set; }
        public object maintenance { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupCrypto
    {
        public object commision { get; set; }
        public object swap_time { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object spread { get; set; }
        public object minimum_step { get; set; }
        public object minimum_amount { get; set; }
        public object leverage { get; set; }
        public object maintenance { get; set; }
        public object margin_call { get; set; }
    }

    public class GetCryptoGroupFutures
    {
        public object commision { get; set; }
        public object swap_time { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object spread { get; set; }
        public object minimum_step { get; set; }
        public object minimum_amount { get; set; }
        public object leverage { get; set; }
        public object maintenance { get; set; }
        public object margin_call { get; set; }
    }
}
