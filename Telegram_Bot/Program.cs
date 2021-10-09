using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;

namespace Telegram_Bot
{
    class Program
    {
        private static string token { get; set; } = "тут токен";
        private static TelegramBotClient client;

        static string NameCity;
        static float tempOfCity;
        static string nameOfCity;
        static bool NotCity;

        static string answerOnWether;

        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            client.StartReceiving();
            client.OnMessage += OnMessageHandler;
            Console.ReadLine();
            client.StopReceiving();

        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            if((msg.Text != null)&(msg.Type == Telegram.Bot.Types.Enums.MessageType.Text))
            {
                Console.WriteLine($"Message come: {msg.Text}");
                
                //await client.SendTextMessageAsync(msg.Chat.Id,msg.Text, replyToMessageId: msg.MessageId);
                switch (msg.Text)
                {
                    case "Все хорошо?":
                        var stick = client.SendStickerAsync(
                        chatId: msg.Chat.Id,
                        sticker: "https://tlgrm.ru/_/stickers/8a1/9aa/8a19aab4-98c0-37cb-a3d4-491cb94d7e12/3.webp",
                        replyToMessageId: msg.MessageId);
                        break;
                    case "Скинь мне Феерверк":
                        var pic = client.SendPhotoAsync(
                            chatId: msg.Chat.Id,
                            photo: "https://i1.wallbox.ru/wallpapers/main/201546/a4df204fdd0b339.jpg",
                            replyMarkup: GetButtons());
                        break;
                    default:
                        NameCity = msg.Text;
                        Weather(NameCity);
                        AnswerToClient(tempOfCity, NameCity);
                        await client.SendTextMessageAsync(msg.Chat.Id, answerOnWether, replyMarkup: GetButtons());
                        break;
                }
            }
        }

        private static void AnswerToClient(float tempOfCity, string NameCity)
        {
            if (!NotCity)
                answerOnWether = "Температура в " + NameCity + ": " + Convert.ToString(Math.Round(tempOfCity)) + " °C";
            else
                answerOnWether = "Нет такого города";
        }

        private static void Weather(string NameCity)
        {
            try
            {
                string url = "https://api.openweathermap.org/data/2.5/weather?q=" + NameCity + "&appid=ebc562d7eebf41fa77eb3033ded3ca24";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest?.GetResponse();
                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);


                nameOfCity = weatherResponse.Name;
                tempOfCity = weatherResponse.Main.Temp -273;

            }
            catch(System.Net.WebException) {
                Console.WriteLine("Не получилось");
                NotCity = true;
                return;
            }

        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Все хорошо?" }, new KeyboardButton { Text = "Скинь мне Феерверк" } },
                    new List<KeyboardButton>{new KeyboardButton { Text = "Moscow" }, new KeyboardButton { Text = "Petersburg" } }
                }
            };
        }
    }
}
