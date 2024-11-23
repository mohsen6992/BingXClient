using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api
{
    public class BingxClient
    {
        public Trade Trade { get; set; }
        public Account Account { get; set; }
        public MarketInterface MarketInterface { get; set; }
        public string SymbolName { get; set; }

        public BingxClient(BingxClientOptions bingxClientOptions)
        {
            Trade = new Trade(bingxClientOptions);
            Account = new Account(bingxClientOptions);
            MarketInterface = new MarketInterface(bingxClientOptions);
            SymbolName = bingxClientOptions.Symbol;
        }
    }
}
