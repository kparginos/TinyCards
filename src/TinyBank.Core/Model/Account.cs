using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TinyBank.Core.Model
{
    public class Account
    {
        public string AccountId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public decimal? Balance { get; set; }
        public Constants.AccountState State { get; set; }
        public Guid CustomerId { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }
        public AuditInfo AuditInfo { get; set; }
        public List<Card> Cards { get; set; }

        public Account()
        {
            AuditInfo = new AuditInfo();
            Cards = new List<Card>();
        }
    }
}
