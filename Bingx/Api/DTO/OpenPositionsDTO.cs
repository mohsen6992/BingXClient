using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
     public class OpenPositionsDTO
    {
        public string OpenInterest { get; set; }
        public string Symbol { get; set; }
        public long time { get; set; }
    }
}
