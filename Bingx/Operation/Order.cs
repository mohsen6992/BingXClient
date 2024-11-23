using Bingx.Api;
using Bingx.Api.DTO;
using Bingx.Api.Model;
using Bingx.MLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bingx.Operation
{
    public static class Order
    {
        public static string symbol = "";
        public static Log log = null;
        public static async Task<MessageError> OrderPostion(BingxClient bingxClient, int quntity, Bingx.Api.Enum.Side side, Api.Enum.PositionSide positionSide, bool isLiquid = false)
        {
            log = new Log();
            int orderCount = 0;
            var orderMessage = new MessageError();
            var order = new OrderResponseDTO();
            long amt = 0;
            try
            {
                var position = await GetAccountPosition(bingxClient);
                if (!position.IsSucess) return new MessageError { IsSucess = false, Message = position.Message };

                if (position.Data.data.Where(c => c.PositionSide == positionSide.ToString()).Any())
                {
                    amt = long.Parse(position.Data.data.Where(c => c.PositionSide == positionSide.ToString()).FirstOrDefault().AvailableAmt.Split('.')[0]);
                }
                else
                {
                    var ex = await CancelOpenOrders(bingxClient, positionSide == Api.Enum.PositionSide.LONG ? true : false);
                    if (!ex.IsSucess)
                    {
                        Telegram.ErrorLogSendMessage(new Log
                        {
                            Date = DateTime.Now,
                            Message = ex.Message,
                            Status = Status.ERROR,
                            Title = ex.Message
                        });
                    }
                }

                while (orderCount <= 2)
                {
                    order = await bingxClient.Trade.PostOrder(new Api.DTO.OrderRequestDTO
                    {
                        Side = side.ToString(),
                        PositionSide = positionSide.ToString(),
                        Type = Api.Enum.OrderType.MARKET.ToString(),
                        Quantity = quntity,
                    });
                    if (order.Msg == "")
                    {
                        Thread.Sleep(5000);
                        position = await GetAccountPosition(bingxClient);
                        var nowAmt = position.Data.data.Where(c => c.PositionSide == positionSide.ToString()).Any() ? long.Parse(position.Data.data.Where(c => c.PositionSide == positionSide.ToString()).FirstOrDefault().AvailableAmt.Split('.')[0]) : 0;

                        if (amt == nowAmt)
                        {
                            orderCount++;
                            order = await bingxClient.Trade.PostOrder(new Api.DTO.OrderRequestDTO
                            {
                                Side = side.ToString(),
                                PositionSide = positionSide.ToString(),
                                Type = Api.Enum.OrderType.MARKET.ToString(),
                                Quantity = quntity
                            });

                        }
                        else
                        {
                            orderCount = 3;
                            orderMessage.IsSucess = true;
                            log.Add(new Log
                            {
                                Message = order.Msg,
                                Status = Status.SUCCESS,
                                Title = "OrderPostion"
                            });

                            string msg = "";
                            msg =
                                positionSide.ToString() + "\n" +
                                "symbol = " + " " + bingxClient.SymbolName + "\n" +
                                 "date = " + " " + DateTime.Now.ToUniversalTime().ToString() + "\n" +
                                "quntity = " + " " + quntity.ToString() + "\n";
                            Telegram.OrderSendMessage(msg);
                        }
                    }
                    else
                    {
                        log = new Log();
                        log.Message = order.Msg;
                        log.Status = Status.ERROR;
                        log.Title = "OrderPostion " + " " + bingxClient.SymbolName;

                        log.Add(log);
                        Telegram.ErrorLogSendMessage(log);
                        orderCount++;
                    }

                    Thread.Sleep(10000);
                }


                if (!orderMessage.IsSucess) return new MessageError { IsSucess = false, Message = order.Msg };

                return orderMessage;
            }
            catch (Exception ex)
            {
                Telegram.ErrorLogSendMessage(new Log
                {
                    Date = DateTime.Now,
                    Message = ex.Message,
                    Status = Status.ERROR,
                    Title = ex.Message
                });
                return new MessageError { IsSucess = false, Message = ex.Message };
            }
        }
        public static async Task<MessageError> OrderTakeProfit(BingxClient bingxClient,
            int quntity, Bingx.Api.Enum.Side side, Bingx.Api.Enum.PositionSide positionSide, decimal price, decimal stopPrice)
        {
            log = new Log();
            int orderCount = 0;
            var orderMessage = new MessageError();
            var order = new OrderResponseDTO();
            quntity = int.Parse(quntity.ToString().Split('.')[0]);
            while (orderCount <= 2)
            {
                order = await bingxClient.Trade.PostOrder(new Api.DTO.OrderRequestDTO
                {
                    Side = side.ToString(),
                    PositionSide = positionSide.ToString(),
                    Type = Bingx.Api.Enum.OrderType.TAKE_PROFIT.ToString(),
                    Price = price,
                    StopPrice = stopPrice,
                    Quantity = quntity
                });

                if (order.Msg == "")
                {

                    orderCount = 3;
                    orderMessage.IsSucess = true;
                    log.Add(new Log
                    {
                        Message = order.Msg,
                        Status = Status.SUCCESS,
                        Title = "OrderTakeProfit"
                    });

                    string msg = "";
                    msg = "TakeProfit" + "\n" +
                        positionSide.ToString() + "\n" +
                        "symbol = " + " " + bingxClient.SymbolName + "\n" +
                         "date = " + " " + DateTime.Now.ToUniversalTime().ToString() + "\n" +
                         "tp = " + " " + price.ToString() + "\n" +
                        "quntity = " + " " + quntity.ToString() + "\n";
                    Telegram.OrderSendMessage(msg);
                }
                else
                {
                    orderCount++;

                    log = new Log();
                    log.Message = order.Msg;
                    log.Status = Status.ERROR;
                    log.Title = "OrderTakeProfit " + " " + bingxClient.SymbolName;

                    log.Add(log);
                    Telegram.ErrorLogSendMessage(log);
                }
            }
            if (!orderMessage.IsSucess) return new MessageError { IsSucess = false, Message = order.Msg };
            return orderMessage;
        }
        public static async Task<MessageError> OrderStopLoss(BingxClient bingxClient,
            int quntity, Bingx.Api.Enum.Side side, Bingx.Api.Enum.PositionSide positionSide, decimal price, decimal stopPrice)
        {
            log = new Log();
            int orderCount = 0;
            var orderMessage = new MessageError();
            var order = new OrderResponseDTO();

            while (orderCount <= 2)
            {
                order = await bingxClient.Trade.PostOrder(new Api.DTO.OrderRequestDTO
                {
                    Side = side.ToString(),
                    PositionSide = positionSide.ToString(),
                    Type = Bingx.Api.Enum.OrderType.STOP.ToString(),
                    Price = price,
                    StopPrice = stopPrice,
                    Quantity = quntity
                });

                if (order.Msg == "")
                {
                    orderCount = 3;
                    orderMessage.IsSucess = true;
                    log.Add(new Log
                    {
                        Message = order.Msg,
                        Status = Status.SUCCESS,
                        Title = "OrderStopLoss"
                    });
                }
                else
                {
                    orderCount++;

                    log = new Log();
                    log.Message = order.Msg;
                    log.Status = Status.ERROR;
                    log.Title = "OrderTakeProfit " + " " + bingxClient.SymbolName;

                    log.Add(log);

                    Telegram.ErrorLogSendMessage(log);
                }
            }
            if (!orderMessage.IsSucess) return new MessageError { IsSucess = false, Message = order.Msg };
            return orderMessage;
        }
        public static async Task<MessageError> BuyPositionAndTakeAndStop(BingxClient bingxClient, int quntity)
        {
            log = new Log();
            log.Add(new Log
            {
                Message = "quntity = " + quntity + "  side=" + Api.Enum.Side.BUY.ToString() + "  PositionSide=" + Api.Enum.PositionSide.LONG.ToString(),
                Status = Status.INFORMATION,
                Title = "OrderPostion"
            });

            var result = await OrderPostion(bingxClient, quntity, Api.Enum.Side.BUY, Api.Enum.PositionSide.LONG);

            if (result.IsSucess)
            {

                log.Add(new Log
                {
                    Message = "quntity = " + quntity + "  side=" + Api.Enum.Side.BUY.ToString() + "  PositionSide=" + Api.Enum.PositionSide.LONG.ToString(),
                    Status = Status.SUCCESS,
                    Title = "OrderPostion"
                });

                Task.Delay(3000).Wait();

                var position = await GetAccountPosition(bingxClient);

                var AvgPrice = decimal.Parse(position.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.LONG.ToString()).FirstOrDefault().AvgPrice.ToString());
                var sl = AvgPrice * 0.99m;
                var tp = AvgPrice * 1.02m;

                var slStop = AvgPrice * 0.99m;
                var tpStop = AvgPrice * 1.02m;


                result = await OrderTakeProfit(bingxClient, quntity, Api.Enum.Side.BUY, Api.Enum.PositionSide.LONG, tp, tpStop);

                log.Add(new Log
                {
                    Message = "tp = " + tp.ToString() + "  sl=" + sl.ToString() + "  slStop=" + slStop.ToString() + " tpStop= " + tpStop,
                    Status = Status.INFORMATION,
                    Title = "OrderTakeProfit"
                });


                if (result.IsSucess)
                {
                    Task.Delay(2000).Wait();
                    result = await OrderStopLoss(bingxClient, quntity, Api.Enum.Side.BUY, Api.Enum.PositionSide.LONG, sl, slStop);

                    string msg = "";
                    msg =
                        "buy" + "\n" +
                        "symbol = " + " " + symbol + "\n" +
                         "date = " + " " + DateTime.Now.ToUniversalTime().ToString() + "\n" +
                        "quntity = " + " " + quntity.ToString() + "\n" +
                        "tp = " + " " + tp.ToString() + "\n" +
                        "sl = " + " " + sl.ToString() + "\n" +
                        "slStop = " + " " + slStop.ToString() + "\n" +
                        "tpStop = " + " " + tpStop.ToString() + "\n";

                    Telegram.OrderSendMessage(msg);
                }
            }

            return result;
        }
        public static async Task<MessageError> SellPositionAndTakeAndStop(BingxClient bingxClient,
            int quntity)
        {
            var result = await OrderPostion(bingxClient, quntity, Api.Enum.Side.SELL, Api.Enum.PositionSide.SHORT);

            log.Add(new Log
            {
                Message = "quntity = " + quntity + "  side=" + Api.Enum.Side.SELL.ToString() + "  PositionSide=" + Api.Enum.PositionSide.SHORT.ToString(),
                Status = Status.INFORMATION,
                Title = "SellPositionAndTakeAndStop"
            });

            if (result.IsSucess)
            {
                Task.Delay(3000).Wait();

                var position = await GetAccountPosition(bingxClient);
                var AvgPrice = decimal.Parse(position.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.SHORT.ToString()).FirstOrDefault().AvgPrice.ToString());
                var sl = AvgPrice * 1.01m;
                var tp = AvgPrice * 0.98m;

                var slStop = AvgPrice * 1.01m;
                var tpStop = AvgPrice * 0.98m;

                result = await OrderTakeProfit(bingxClient, quntity, Api.Enum.Side.SELL, Api.Enum.PositionSide.SHORT, tp, tpStop);


                log.Add(new Log
                {
                    Message = "tp = " + tp.ToString() + "  sl=" + sl.ToString() + "  slStop=" + slStop.ToString() + " tpStop= " + tpStop,
                    Status = Status.INFORMATION,
                    Title = "SellPositionAndTakeAndStop"
                });

                if (result.IsSucess)
                {
                    Task.Delay(2000).Wait();
                    result = await OrderStopLoss(bingxClient, quntity, Api.Enum.Side.SELL, Api.Enum.PositionSide.SHORT, sl, slStop);

                    string msg = "";
                    msg =
                     "sell" + "\n" +
                       "symbol = " + " " + symbol + "\n" +
                      "date = " + " " + DateTime.Now.ToUniversalTime().ToString() + "\n" +
                     "quntity = " + " " + quntity.ToString() + "\n" +
                     "tp = " + " " + tp.ToString() + "\n" +
                     "sl = " + " " + sl.ToString() + "\n" +
                     "slStop = " + " " + slStop.ToString() + "\n" +
                     "tpStop = " + " " + tpStop.ToString() + "\n";

                    Telegram.OrderSendMessage(msg);
                }
            }

            return result;
        }
        public static async Task<MessageError> buyAndSell(BingxClient bingxClient, int quntity)
        {
            var result = await OrderPostion(bingxClient, quntity, Api.Enum.Side.BUY, Api.Enum.PositionSide.LONG);
            result = await OrderPostion(bingxClient, quntity, Api.Enum.Side.SELL, Api.Enum.PositionSide.SHORT);

            return new MessageError
            {
                IsSucess = true,
                Message = "",
            };
        }
        public static async Task<bool> SWitchLeverageSide(BingxClient bingxClient, long leverage, Api.Enum.PositionSide side)
        {
            var data = await bingxClient.Trade.SwitchLeverage(new Api.DTO.SwitchLeverageRequestDTO
            {
                Leverage = leverage,
                Side = side.ToString()
            });

            if (data.Msg == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<bool> SWitchLeverage(BingxClient bingxClient, long leverage)
        {
            var sucess = await SWitchLeverageSide(bingxClient, leverage, Api.Enum.PositionSide.LONG);
            sucess = await SWitchLeverageSide(bingxClient, leverage, Api.Enum.PositionSide.SHORT);
            return sucess;
        }
        public static async Task<MessageError> ClosePosition(BingxClient bingxClient)
        {
            _ = await bingxClient.Trade.PostCloseAllPositions();
            return new MessageError
            {
                IsSucess = true,
                Message = "",
            };
        }
        public static async Task<MessageError> CancelAllOpenOrders(BingxClient bingxClient)
        {
            _ = await bingxClient.Trade.CancelAllOrder();
            return new MessageError
            {
                IsSucess = true,
                Message = "",
            };
        }
        public static async Task<MessageError<AccountPositionsResponseDTO>> GetAccountPosition(BingxClient bingxClient)
        {
            try
            {
                AccountPositionsResponseDTO postion = null;
                do
                {
                    Thread.Sleep(3000);
                    postion = await bingxClient.Account.AccountPosition();
                } while (postion.msg != "");
                return new MessageError<AccountPositionsResponseDTO> { Data = postion, IsSucess = true, Message = "" };
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log = new Log();
                log.Message = ex.Message;
                log.Status = Status.ERROR;
                log.Title = "GetAccountPosition " + " " + symbol;

                log.Add(log);

                return new MessageError<AccountPositionsResponseDTO>
                {
                    Data = null,
                    IsSucess = false,
                    Message = ex.Message.ToString(),
                };

            }

        }
        public static async Task<MessageError> CancelAnPostionOrder(BingxClient bingxClient)
        {
            var accountPosition = await GetAccountPosition(bingxClient);
            if (accountPosition.Data.data.Any())
            {
                try
                {
                    if (accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.LONG.ToString()).Any())
                    {
                        var data = accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.LONG.ToString()).FirstOrDefault();
                        var result = await OrderPostion(bingxClient, int.Parse(data.AvailableAmt.Split('.')[0]), Api.Enum.Side.SELL, Api.Enum.PositionSide.LONG);

                        if (!result.IsSucess)
                        {
                            int counter = 0;
                            while (!result.IsSucess)
                            {
                                Thread.Sleep(5000);
                                result = await OrderPostion(bingxClient, int.Parse(data.AvailableAmt.Split('.')[0]), Api.Enum.Side.SELL, Api.Enum.PositionSide.LONG);
                                if (result.IsSucess)
                                {
                                    log.Add(new Log
                                    {
                                        Message = "close Postion Long",
                                        Title = "close Postion",
                                        Status = Status.SUCCESS
                                    }); ;
                                    Telegram.LogSendMessage(log.Message);
                                }
                                else
                                {
                                    log = new Log();
                                    log.Message = result.Message;
                                    log.Title = "close  Long";
                                    log.Status = Status.ERROR;
                                    log.Add(log);
                                    Telegram.ErrorLogSendMessage(log);
                                    counter++;
                                    Thread.Sleep(10000);
                                    if (counter == 3) result.IsSucess = true;
                                }
                            }
                        }
                        else
                        {
                            log.Add(new Log
                            {
                                Message = "close Postion Short",
                                Title = "close Postion",
                                Status = Status.SUCCESS
                            }); ;
                            Telegram.LogSendMessage(log.Message);
                        }
                    }
                    else if (accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.SHORT.ToString()).Any())
                    {
                        var data = accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.SHORT.ToString()).FirstOrDefault();
                        var result = await OrderPostion(bingxClient, int.Parse(data.AvailableAmt.Split('.')[0]), Api.Enum.Side.BUY, Api.Enum.PositionSide.SHORT);
                        if (!result.IsSucess)
                        {
                            int counter = 0;
                            while (!result.IsSucess)
                            {
                                Thread.Sleep(5000);
                                result = await OrderPostion(bingxClient, int.Parse(data.AvailableAmt.Split('.')[0]), Api.Enum.Side.BUY, Api.Enum.PositionSide.SHORT);
                                if (result.IsSucess)
                                {
                                    log.Add(new Log
                                    {
                                        Message = "close Postion Short",
                                        Title = "close Postion",
                                        Status = Status.SUCCESS
                                    }); ;
                                    Telegram.LogSendMessage(log.Message);
                                }
                                else
                                {
                                    log = new Log();
                                    log.Message = result.Message;
                                    log.Title = "close  Short";
                                    log.Status = Status.ERROR;
                                    log.Add(log);
                                    Telegram.ErrorLogSendMessage(log);
                                    Thread.Sleep(10000);
                                    counter++;
                                    if (counter == 3) result.IsSucess = true;
                                }
                            }
                        }
                        else
                        {
                            log.Add(new Log
                            {
                                Message = "close Postion Short",
                                Title = "close Postion",
                                Status = Status.SUCCESS
                            }); ;
                            Telegram.LogSendMessage(log.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Telegram.ErrorLogSendMessage(new Log
                    {
                        Date = DateTime.Now,
                        Message = ex.Message,
                        Status = Status.ERROR,
                        Title = "CancelAnPostionOrder"
                    });
                    log.Add(new Log
                    {
                        Date = DateTime.Now,
                        Message = ex.Message,
                        Status = Status.ERROR,
                        Title = "CancelAnPostionOrder"
                    });
                }

            }

            return new MessageError { IsSucess = true, Message = "" };
        }
        public static async Task<MessageError> CheckedLiquid(BingxClient bingxClient, decimal price)
        {
            var accountPosition = await GetAccountPosition(bingxClient);
            if (accountPosition.Data.data.Any())
            {
                if (accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.LONG.ToString()).Any())
                {
                    var data = accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.LONG.ToString()).FirstOrDefault();
                    var tp = decimal.Parse(data.AvgPrice) * 1.0416m;
                    var sl = decimal.Parse(data.AvgPrice) * 0.98m;
                    if (price >= tp || price <= sl)
                    {

                        var result = await OrderPostion(bingxClient, int.Parse(data.AvailableAmt.Split('.')[0]), Api.Enum.Side.SELL, Api.Enum.PositionSide.LONG);
                        if (result.IsSucess)
                        {
                            log.Add(new Log
                            {
                                Message = "CheckedLiquid tp SELL  LONG " + (price >= tp ? "tp" : "sl") + " " + tp.ToString() + "  " + sl.ToString(),
                                Title = "CheckedLiquid Long",
                                Status = Status.SUCCESS
                            }); ;

                        }
                        else
                        {
                            log = new Log();
                            log.Message = result.Message;
                            log.Title = "CheckedLiquid  Long";
                            log.Status = Status.ERROR;
                            log.Add(log);

                            Telegram.ErrorLogSendMessage(log);
                        }

                    }
                }
                if (accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.SHORT.ToString()).Any())
                {
                    var data = accountPosition.Data.data.Where(c => c.PositionSide == Api.Enum.PositionSide.SHORT.ToString()).FirstOrDefault();
                    var tp = decimal.Parse(data.AvgPrice) * 0.9584m;
                    var sl = decimal.Parse(data.AvgPrice) * 1.02m;
                    if (price <= tp || price >= sl)
                    {
                        var result = await OrderPostion(bingxClient, int.Parse(data.AvailableAmt.Split('.')[0]), Api.Enum.Side.BUY, Api.Enum.PositionSide.SHORT);

                        if (result.IsSucess)
                        {
                            log.Add(new Log
                            {
                                Message = "CheckedLiquid  BUY  SHORT " + (price >= tp ? "tp" : "sl") + " " + tp.ToString() + "  " + sl.ToString(),
                                Title = "CheckedLiquid Short",
                                Status = Status.SUCCESS
                            });
                        }
                        else
                        {

                            log = new Log();
                            log.Message = result.Message;
                            log.Title = "CheckedLiquid  Short";
                            log.Status = Status.ERROR;
                            log.Add(log);

                            Telegram.ErrorLogSendMessage(log);
                        }
                    }
                }
            }
            return new MessageError
            {
                IsSucess = false,
                Message = ""
            };
        }
        public static async Task<MessageError> CancelAnOpenOrders(BingxClient bingxClient)
        {
            _ = await bingxClient.Trade.CancelAnOrder(new CancelAnOrderRequestDTO{});

            return new MessageError
            {
                IsSucess = true,
                Message = "",
            };
        }
        public static async Task<MessageError> CancelOpenOrders(BingxClient bingxClient, bool isBuy)
        {
            var msg = new MessageError();
            int counter = 0;
            while (counter <= 2)
            {
                try
                {
                    var data = await bingxClient.Trade.QueryAllCurrentPendingOrders();
                    if (data.msg == "")
                    {
                        if (data.data.orders.Any())
                        {
                            foreach (var item in data.data.orders.Where(c => c.PositionSide == (isBuy ? Api.Enum.PositionSide.LONG.ToString() : Api.Enum.PositionSide.SHORT.ToString())))
                            {
                                Task.Delay(1500).Wait();
                                var result = await bingxClient.Trade.CancelAnOrder(new CancelAnOrderRequestDTO
                                {
                                    orderId = item.OrderId
                                });

                                if (result.Msg == "")
                                {
                                    msg.IsSucess = true;
                                    msg.Message = "";
                                    counter = 3;
                                }
                                else
                                {
                                    counter++;
                                    if (counter >= 3)
                                    {
                                        msg.IsSucess = true;
                                        msg.Message = "";
                                    }
                                }
                            }
                        }
                        else
                        {
                            msg.IsSucess = true;
                            msg.Message = "";
                            counter = 3;
                        }
                    }
                    else
                    {
                        counter++;
                        if (counter == 3)
                        {
                            msg.IsSucess = true;
                            msg.Message = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Telegram.ErrorLogSendMessage(new Log { Message = ex.Message, Title = "CancelOpenOrders", Date = DateTime.Now.ToUniversalTime() });
                    counter++;
                    if (counter >= 3)
                    {
                        msg.IsSucess = true;
                        msg.Message = "";
                    }
                }
            }

            msg.IsSucess = true;
            msg.Message = "";
            return msg;
        }

    }
}
