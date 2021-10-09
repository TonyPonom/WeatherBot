using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram_Bot
{
    public class WeatherResponse
    {
        public TemeraureInfo Main { get; set; }

        public string Name { get; set; }
    }
}
