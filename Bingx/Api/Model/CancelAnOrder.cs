using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class CancelAnOrder
    {
        public long Time { get; set; }
        public string Symbol { get; set; }
        public string Side { get; set; }
        public string Type { get; set; }
        public string PositionSide { get; set; }
        public string CumQuote { get; set; }
        public string Status { get; set; }
        public string StopPrice { get; set; }
        public string Price { get; set; }
        public string OrigQty { get; set; }
        public string AvgPrice { get; set; }
        public string ExecutedQty { get; set; }
        public long OrderId { get; set; }
        public string Profit { get; set; }
        public string Commission { get; set; }
        public long UpdateTime { get; set; }
        public string ClientOrderID { get; set; }
    }
}
