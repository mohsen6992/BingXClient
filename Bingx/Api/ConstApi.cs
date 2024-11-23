using System;
using System.Collections.Generic;
using System.Text;

namespace Bingx.Api
{
    public static class ConstApi
    {
        public static string contracts = "/openApi/swap/v2/quote/contracts";
        public static string klines = "/openApi/swap/v3/quote/klines";
        public static string time = "/openApi/swap/v2/server/time";
        public static string Get = "GET";
        public static string POST = "POST";
        public static string DELETE = "DELETE";
        public static string PUT = "PUT";
        public static string order = "/openApi/swap/v2/trade/order";
        public static string close_all_positions = "/openApi/swap/v2/trade/closeAllPositions";
        public static string cancel_all_orders = "/openApi/swap/v2/trade/allOpenOrders";
        public static string trade_order_test = "/openApi/swap/v2/trade/order/test";
        public static string get_swap_open_positions = "/openApi/swap/v2/quote/openInterest";
        public static string balance = "/openApi/swap/v2/user/balance";
        public static string positions = "/openApi/swap/v2/user/positions";
        public static string incom = "/openApi/swap/v2/user/income";
        public static string openOrders = "/openApi/swap/v2/trade/openOrders";
        public static string allOpenOrders = "/openApi/swap/v2/trade/allOpenOrders";
        public static string marginType = "/openApi/swap/v2/trade/marginType";
        public static string leverage = "/openApi/swap/v2/trade/leverage";
    }
}
