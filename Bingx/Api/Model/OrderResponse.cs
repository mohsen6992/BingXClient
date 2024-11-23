using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class OrderResponse
    {
        public string Symbol { get; set; }
        public string Side { get; set; }
        public string Type { get; set; }
        public string PositionSide { get; set; }
        public long OrderId { get; set; }
        public string ClientOrderID { get; set; }
    }
}
