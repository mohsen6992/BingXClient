using Bingx.Api.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bingx.Api
{
    public  class Account : BingxRestClient
    {
        public Account(BingxClientOptions bingxClientOptions) : base(bingxClientOptions)
        { }

        public async Task<BalanceDTO> Balance()
        {
            var data = await SendRequest<BalanceDTO>(ConstApi.Get, ConstApi.balance, new { });
            return data;
        }
        public async Task<AccountPositionsResponseDTO> AccountPosition()
        {
            var data = await SendRequest<AccountPositionsResponseDTO>(ConstApi.Get, ConstApi.positions, new { symbol = bingxClientOptions.Symbol });
            return data;
        }
        public async Task<IncomeResponseDTO> GetIncome(IncomeRequestDTO requst)
        {
            var data = await SendRequest<IncomeResponseDTO>(ConstApi.Get, ConstApi.incom, requst);
            return data;
        }
        public async Task<ServerTimeDTO> GetServerTime()
        {
            var data = await SendRequest<ServerTimeDTO>(ConstApi.Get, ConstApi.time, new { });
            TimeSpan time = TimeSpan.FromMilliseconds(data.Data.ServerTime);
            DateTime startdate = new DateTime(time.Ticks);

            return data;
        }
    }
}
