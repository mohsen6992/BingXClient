using Bingx.Api.Enum;
using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bingx.Api.DTO
{
    public class OrderRequestDTO
    {
        public string Symbol { get; set; }
        public string Type { get; set; }
        public string Side { get; set; }
        public string PositionSide { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal StopPrice { get; set; }
        public decimal PriceRate { get; set; }
        [Description("StopPrice trigger price types: MARK_PRICE, CONTRACT_PRICE, INDEX_PRICE, default MARK_PRICE")]
        public string WorkingType { get; set; }
        public int Timestamp { get; set; }
        public string ClientOrderID { get; set; }

        public string takeProfit { get; set; }

    }

    public class OrderResponseDTO
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public OrderResponse Data { get; set; }
    }
}
