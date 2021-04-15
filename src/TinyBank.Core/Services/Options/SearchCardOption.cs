using System;

namespace TinyBank.Core.Services.Options
{
    public class SearchCardOption
    {
        public string CardNumber { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public int? MaxResults { get; set; }
        public bool? TrackResults { get; set; }
        public int? Skip { get; set; }
    }
}
