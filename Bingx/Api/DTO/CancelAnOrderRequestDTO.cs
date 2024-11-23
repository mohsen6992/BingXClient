using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
    public class CancelAnOrderRequestDTO
    {
        public long orderId { get; set; }
        public string symbol { get; set; }
        public long clientOrderID { get; set; }
    }

    public class CancelOrder
    {
        public List<CancelAnOrder> success { get; set; }
        public Array failed { get; set; }
    }

    public class CancelAnOrderResponseDTO
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public CancelOrder Data { get; set; }
    }
}
