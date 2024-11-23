using Bingx.Api.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bingx.Api.Model
{
    public class IncomeResponse
    {
        public string Symbol { get; set; }
        public IncomeType IncomeType { get; set; }
        [Description("The amount of capital flow, positive numbers represent inflows, negative numbers represent outflow")]
        public string Income { get; set; }
        public string Asset { get; set; }
        public string Info { get; set; }
        public long Time { get; set; }
        public string TranId { get; set; }
        public string TradeId { get; set; }
    }
}
