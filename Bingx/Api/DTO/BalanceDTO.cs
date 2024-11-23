using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
    public class BalanceDTO
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public Balance data { get; set; }
    }
}
