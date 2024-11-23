using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class KlinesResponse
    {
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
        public long Time { get; set; }
        public DateTime Date { get; set; }
    }
}
