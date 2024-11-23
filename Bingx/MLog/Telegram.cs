using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Bingx.MLog
{
    public class Telegram
    {
        public const string token = "******";
        public static void LogSendMessage(string message)
        {
            string chatId = "";
            Send(chatId, message);
        }
        public static void ErrorLogSendMessage(Log log)
        {
            string chatId = "";
            Send(chatId, "", log);
        }
        public static void OrderSendMessage(string message)
        {
            string chatId = "";
            Send(chatId, message);
        }
        private static void Send(string chatId, string msg, Log log = null)
        {
            try
            {
                string url = "";
                if (msg != "")
                {
                    url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chatId}&text={msg}";
                }
                else
                {
                    msg = DateTime.Now.ToUniversalTime().ToString() + "\n" +
                        log.Title.ToString() + "\n" +
                        log.Message.ToString();
                    url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chatId}&text={msg}";
                }

                using (var webClient = new WebClient())
                {
                    webClient.DownloadString(url);
                }
            }
            catch
            {
            }
        }

    }
}
