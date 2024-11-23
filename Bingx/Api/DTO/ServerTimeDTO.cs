using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
    public class ServerTimeDTO
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public Time Data { get; set; }
    }

    public class Time
    {
        public long ServerTime { get; set; }
    }
}
