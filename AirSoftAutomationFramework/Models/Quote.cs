using System;
using System.Collections.Generic;

namespace ConsoleApp;

public partial class Quote
{
    public int Id { get; set; }

    public string AssetSymbol { get; set; }

    public float SellPrice { get; set; }

    public float BuyPrice { get; set; }

    public int DecimalDigits { get; set; }
}
