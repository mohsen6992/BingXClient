using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class BalanceCustomer
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public string Asset { get; set; }
        public string Balance { get; set; }
        public string Equity { get; set; }
        public string UnrealizedProfit { get; set; }
        public string RealisedProfit { get; set; }
        public string AvailableMargin { get; set; }
        public string UsedMargin { get; set; }
        public string FreezedMargin { get; set; }
    }

    public class Balance
    {
        public BalanceCustomer balance { get; set; }
    }
}
