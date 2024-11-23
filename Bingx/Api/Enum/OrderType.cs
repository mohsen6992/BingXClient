using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api.Enum
{
    public enum OrderType
    {
        LIMIT,
        MARKET,
        TRAILING_STOP_MARKET,
        TRIGGER_LIMIT,
        STOP,
        TAKE_PROFIT,
        STOP_MARKET,
        TAKE_PROFIT_MARKET,
        TRIGGER_MARKET
    }
}
