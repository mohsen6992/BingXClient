using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
   

    public class KlinesRequestDTO
    {
        public string Symbol { get; set; }
        public string Interval { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long Limit { get; set; }
    }

    public class KlinesResponseDTO
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public List<KlinesResponse> Data { get; set; }
    }

}
