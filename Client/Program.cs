using Bingx.Api;
using Bingx.Api.Enum;


var bingxClientOptions = new BingxClientOptions
{
    API_KEY = "*********************",
    API_SECRET = "**********************",
    Symbol = "PEOPLE-USDT"
};

BingxClient client = new BingxClient(bingxClientOptions);
var data = await Bingx.Operation.Price.GetPrice(client,  TimeFrame.OneHour,DateTime.Now , DateTime.Now.AddDays(15) , 1000);
