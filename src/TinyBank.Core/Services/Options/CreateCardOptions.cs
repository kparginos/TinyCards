using System;

namespace TinyBank.Core.Services.Options
{
    public class CreateCardOptions
    {
        public string CardNumber { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public bool Active { get; set; }
        public Constants.CardType CardType { get; set; }
    }
}
