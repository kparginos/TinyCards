using System;

namespace TinyBank.Core.Services.Options
{
    public class SearchAccountOptions
    {
        public string AccountId { get; set; }
        public Guid CustomerId { get; set; }
        public int? MaxResults { get; set; }
        public bool? TrackResults { get; set; }
        public int? Skip { get; set; }
    }
}
