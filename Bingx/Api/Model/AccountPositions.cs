using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Model
{
    public class AccountPositions
    {
        public string Symbol { get; set; }
        public string PositionId { get; set; }
        public string PositionSide { get; set; }
        public bool Isolated { get; set; }
        public string PositionAmt { get; set; }
        public string AvailableAmt { get; set; }
        public string UnrealizedProfit { get; set; }
        public string RealisedProfit { get; set; }
        public string InitialMargin { get; set; }
        public string AvgPrice { get; set; }
        public int Leverage { get; set; }
        public string Margin { get; set; }

    }
}
