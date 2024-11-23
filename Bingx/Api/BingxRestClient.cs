using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bingx.Api.DTO;
using Bingx.MLog;
using Newtonsoft.Json;

namespace Bingx.Api
{
    public class BingxRestClient
    {
        string HOST = "open-api.bingx.com";
        string DemoHost = "open-api-vst.bingx.com";
        string protocol = "https";
        protected BingxClientOptions bingxClientOptions { get; set; }
        protected BingxRestClient(BingxClientOptions _bingxClientOptions)
        {
            bingxClientOptions = _bingxClientOptions;
        }
        protected async Task<T> SendRequest<T>(string method , string api , object payload) where T : class, new()
        {
            Log log = new Log();
            try
            {
                long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string parameters = $"timestamp={timestamp}";
                string responseBody = "";

                if (payload != null)
                {
                    foreach (var property in payload.GetType().GetProperties())
                    {
                        parameters += $"&{property.Name.Substring(0,1).ToLower() + property.Name.Substring(1, property.Name.Length - 1) }={property.GetValue(payload)}";
                    }
                }

                string sign = CalculateHmacSha256(parameters, bingxClientOptions.API_SECRET);
                string url = $"{protocol}://{HOST}{api}?{parameters}&signature={sign}";
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                    using (HttpClient client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Add("X-BX-APIKEY", bingxClientOptions.API_KEY);
                        HttpResponseMessage response;

                        if (method.ToUpper() == ConstApi.Get)
                        {
                            response = await client.GetAsync(url);
                        }
                        else if (method.ToUpper() == ConstApi.POST)
                        {
                            response = await client.PostAsync(url, null);
                        }
                        else if (method.ToUpper() == ConstApi.DELETE)
                        {
                            response = await client.DeleteAsync(url);
                        }
                        else if (method.ToUpper() == ConstApi.PUT)
                        {
                            response = await client.PutAsync(url, null);
                        }
                        else
                        {
                            throw new NotSupportedException("Unsupported HTTP method: " + method);
                        }

                        response.EnsureSuccessStatusCode();
                        responseBody = await response.Content.ReadAsStringAsync();

                     
                    }
                }
                

                if (JsonConvert.DeserializeObject<DataMsg>(responseBody.ToString()).Msg !="")
                {
                    log.Add(new Log
                    {
                        Date = DateTime.Now,
                        Message = responseBody.ToString(),
                        Title = "SendRequest " + " " + url,
                        Status = Status.ERROR,
                    });
                    responseBody = responseBody.Replace("{}", "[]");
                }

                var data = JsonConvert.DeserializeObject<T>(responseBody);
                return data;
            }
            catch (Exception ex)
            {
                log.Add(new Log
                {
                    Date = DateTime.Now,
                    Message = ex.Message.ToString(),
                    Title = "SendRequest",
                    Status = Status.ERROR,
                });

                return new T();
            }
        }


        protected static string CalculateHmacSha256(string input, string key)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

    }
}
