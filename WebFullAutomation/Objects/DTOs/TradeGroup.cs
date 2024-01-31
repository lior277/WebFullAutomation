using Newtonsoft.Json;
using System;

namespace AirSoftAutomationFramework.Objects.DTOs
{
    public class TradeGroup
    {
        public string _id { get; set; }
        public Assets assets { get; set; }
        public Forex forex { get; set; }
        public Commodities commodities { get; set; }
        public Stock stock { get; set; }
        public Indices indices { get; set; }
        public string name { get; set; }
        public Default_Attr default_attr { get; set; }
        public bool @default { get; set; }
        public DateTime created_at { get; set; }
        public Stk stk { get; set; }
        public Cash cash { get; set; }
        public Cmdty cmdty { get; set; }
        public Ind ind { get; set; }
        public Crypto crypto { get; set; }
        public Futures futures { get; set; }
        public DateTime last_update { get; set; }
        public int revision { get; set; }
    }
    public class Assets
    {
        public Apple APPLE { get; set; }
    }

    public class CryptoGroupADAUSD
    {
        public int? commision { get; set; }
        public int? leverage { get; set; }
        public int? maintenance { get; set; }
        public int? minimum_amount { get; set; }
        public int? minimum_step { get; set; }
        public double spread { get; set; }
        public double? swap_long { get; set; }
        public double? swap_short { get; set; }
        public string swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class Forex
    {
        public double? commision { get; set; }
        public int? leverage { get; set; }
        public object maintenance { get; set; }
        public int? minimum_amount { get; set; }
        public int? minimum_step { get; set; }
        public double? spread { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class Commodities
    {
        public object commision { get; set; }
        public int? leverage { get; set; }
        public object maintenance { get; set; }
        public object minimum_amount { get; set; }
        public object minimum_step { get; set; }
        public double? spread { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class Stock
    {
        public object commision { get; set; }
        public int? leverage { get; set; }
        public object maintenance { get; set; }
        public object minimum_amount { get; set; }
        public object minimum_step { get; set; }
        public double? spread { get; set; }
        public object swap_long { get; set; }
        public object swap_short { get; set; }
        public object swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class Apple
    {
        public int? commision { get; set; }
        public double? leverage { get; set; }
        public int? maintenance { get; set; }
        public int? minimum_amount { get; set; }
        public int? minimum_step { get; set; }
        public double? spread { get; set; }
        public double? swap_long { get; set; }
        public double? swap_short { get; set; }
        public string swap_time { get; set; }
        public double? margin_call { get; set; }
    }

    public class Indices
    {
        public int? commision { get; set; }
        public int? leverage { get; set; }
        public int? maintenance { get; set; }
        public int? minimum_amount { get; set; }
        public int? minimum_step { get; set; }
        public double? spread { get; set; }
        public double? swap_long { get; set; }
        public double? swap_short { get; set; }
        public string swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class Default_Attr
    {
        public double commision { get; set; }
        public int leverage { get; set; }
        public double maintenance { get; set; }
        public int minimum_amount { get; set; }
        public double minimum_step { get; set; }
        public object margin_call { get; set; }
        public double spread { get; set; }
        public double swap_long { get; set; }
        public double swap_short { get; set; }
        public string swap_time { get; set; }
    }

    public class Stk
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

    public class Cash
    {
        public double? commision { get; set; }
        public double? leverage { get; set; }
        public double? maintenance { get; set; }
        public double? minimum_amount { get; set; }
        public double? minimum_step { get; set; }
        public double? spread { get; set; }
        public double? swap_long { get; set; }
        public double? swap_short { get; set; }
        public string swap_time { get; set; }
        public object margin_call { get; set; }
    }

    public class Cmdty
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

    public class Ind
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

    public class Crypto
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

    public class Futures
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

