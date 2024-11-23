using Bingx.Api.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bingx.Api
{
    public class MarketInterface : BingxRestClient
    {
        public MarketInterface(BingxClientOptions bingxClientOptions) : base(bingxClientOptions)
        {
        }

        public async Task<List<ContractInformationDTO>> ContractInformation()
        {
            var data = await SendRequest<List<ContractInformationDTO>>(ConstApi.Get, ConstApi.contracts, new { });
            return data;
        }
        public async Task<KlinesResponseDTO> GetKline(KlinesRequestDTO klinesRequestDTO)
        {
            if (klinesRequestDTO.StartDate != null)
                klinesRequestDTO.StartTime =(long)klinesRequestDTO.StartDate.Value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            if (klinesRequestDTO.EndDate != null)
                klinesRequestDTO.EndTime = (long)klinesRequestDTO.EndDate.Value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; ;

            klinesRequestDTO.Symbol = bingxClientOptions.Symbol;

            var data = await SendRequest<KlinesResponseDTO>(ConstApi.Get, ConstApi.klines, klinesRequestDTO);
            return data;
        }
        public async Task<List<OpenPositionsDTO>> GetOpenPosition(string symbol)
        {
            var data = await SendRequest<List<OpenPositionsDTO>>(ConstApi.Get, ConstApi.get_swap_open_positions, symbol);
            return data;
        }

    }
}
