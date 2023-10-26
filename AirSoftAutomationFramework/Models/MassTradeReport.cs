using System;
using System.Collections.Generic;

namespace ConsoleApp;

public partial class MassTradeReport
{
    public int Id { get; set; }

    public int? MassTradeId { get; set; }

    public string Error { get; set; }

    public bool? Success { get; set; }

    public string UserId { get; set; }
}
