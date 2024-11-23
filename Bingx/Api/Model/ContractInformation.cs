using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class ContractInformation
    {
        public string ContractId { get; set; }
        public string Symbol { get; set; }
        public string Size { get; set; }
        public int QuantityPrecision { get; set; }
        public int PricePrecision { get; set; }
        public long FeeRate { get; set; }
        public int TradeMinLimit { get; set; }
        public string Currency { get; set; }
        public string Asset { get; set; }
        public int Status { get; set; }
        public int MaxLongLeverage { get; set; }
        public int MaxShortLeverage { get; set; }
    }
}
