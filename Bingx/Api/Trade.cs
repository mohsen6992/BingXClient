using Bingx.Api.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bingx.Api
{
    public class Trade : BingxRestClient
    {
        public Trade(BingxClientOptions bingxClientOptions) : base(bingxClientOptions)
        { }
        public async Task<OrderResponseDTO> PostOrder(OrderRequestDTO request)
        {
            request.Symbol = bingxClientOptions.Symbol;
            var data = await SendRequest<OrderResponseDTO>(ConstApi.POST, ConstApi.order, request);
            return data;
        }
        public async Task<CloseAllPositionsDTO> PostCloseAllPositions()
        {
            var data = await SendRequest<CloseAllPositionsDTO>(ConstApi.POST, ConstApi.close_all_positions, new { });
            return data;
        }
        public async Task<CancelAnOrderResponseDTO> CancelAnOrder(CancelAnOrderRequestDTO requst)
        {
            requst.symbol = bingxClientOptions.Symbol;
            var data = await SendRequest<CancelAnOrderResponseDTO>(ConstApi.DELETE, ConstApi.order, requst);
            return data;
        }
        public async Task<CancelAnOrderResponseDTO> CancelAllOrder()
        {
            var data = await SendRequest<CancelAnOrderResponseDTO>(ConstApi.DELETE, ConstApi.allOpenOrders, new { symbol = bingxClientOptions.Symbol });
            return data;
        }
        public async Task<OpenOrdersDTO> QueryAllCurrentPendingOrders()
        {
            var data = await SendRequest<OpenOrdersDTO>(ConstApi.Get, ConstApi.openOrders, new { symbol = bingxClientOptions.Symbol });
            return data;
        }
        public async Task<OpenOrdersDTO> QueryOrder()
        {
            var data = await SendRequest<OpenOrdersDTO>(ConstApi.Get, ConstApi.openOrders, new { symbol = bingxClientOptions.Symbol });
            return data;
        }
        public async Task<OrderResponseDTO> QueryMarginMode(OrderRequestDTO request)
        {
            var data = await SendRequest<OrderResponseDTO>(ConstApi.POST, ConstApi.trade_order_test, request);
            return data;
        }

        public async Task<SwitchLeverageResponseDTO> SwitchLeverage(SwitchLeverageRequestDTO request)
        {
            request.Symbol = bingxClientOptions.Symbol;
            var data = await SendRequest<SwitchLeverageResponseDTO>(ConstApi.POST, ConstApi.leverage, request);
            return data;
        }


    }
}
