using Bingx.Api;
using Bingx.Api.DTO;
using Bingx.Api.Model;
using Bingx.MLog;
using IronPython.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static IronPython.Runtime.Profiler;

namespace Bingx.Operation
{
    public static class Price
    {
        public static async Task<MessageError<KlinesResponseDTO>> GetPrice(BingxClient bingxClient, string Interval, DateTime startDate, DateTime endDate, int limit = 500)
        {
            MessageError<KlinesResponseDTO> messageError = null;
            var getKline = await bingxClient.MarketInterface.GetKline(new Bingx.Api.DTO.KlinesRequestDTO { Interval = Interval, Limit = limit, StartDate = startDate, EndDate = endDate });

            if (getKline.Msg == "")
            {
                getKline.Data.ForEach(c => c.Date = ConvertTotalToMilisecend(c.Time));
                messageError = new MessageError<KlinesResponseDTO> { IsSucess = true, Message = "", Data = getKline };
            }
            else
            {
                Log log = new Log();
                log.Add(new Log
                {
                    Message = getKline.Msg,
                    Status = Status.ERROR,
                    Title = "GetPrice",
                });

                messageError = new MessageError<KlinesResponseDTO> { IsSucess = false, Message = getKline.Msg, Data = null };
            }

            return messageError;
        }
        private static DateTime ConvertTotalToMilisecend(long time)
        {
            double ticks = double.Parse(time.ToString());
            TimeSpan times = TimeSpan.FromMilliseconds(ticks);
            DateTime date = new DateTime(1970, 1, 1) + times;
            return date;
        }
    }
}
