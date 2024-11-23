using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
    public class Order
    {
        public List<OpenOrders> orders { get; set; }
    }
    public class OpenOrdersDTO
    {
        public long code { get; set; }
        public string msg { get; set; }
        public Order data { get; set; }
    }
}
