using Bingx.Api.Enum;
using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Bingx.Api.DTO
{
    public class IncomeRequestDTO
    {
        public string Symbol { get; set; }
        public IncomeType IncomeType { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public long Limit { get; set; }
        public long Timestamp { get; set; }

    }


    public class IncomeResponseDTO
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public List<IncomeResponse> Data { get; set; }
    }
   
}
