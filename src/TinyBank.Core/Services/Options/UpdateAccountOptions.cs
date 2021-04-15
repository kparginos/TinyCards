namespace TinyBank.Core.Services.Options
{
    public class UpdateAccountOptions
    {
        public string AccountId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public decimal? Balance { get; set; }
        public Constants.AccountState State { get; set; }
    }
}
