using Bingx.Api.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.DTO
{
    public class SwitchLeverageRequestDTO
    {
        public string Symbol { get; set; }
        public string Side { get; set; }
        public long Leverage { get; set; }

    }
    public class SwitchLeverageResponseDTO
    {
        public long Code { get; set; }
        public string Msg { get; set; }
        public SwitchLeverage Data { get; set; }
    }
}
