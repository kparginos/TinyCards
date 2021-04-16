using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TinyBank.Core.Constants;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using Xunit;

namespace TinyBank.Core.Tests
{
    public class CardTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly ICardService _cards;
        private readonly IAccountService _accounts;
        private readonly ICustomerService _customers;

        public CardTests(TinyBankFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _cards = fixture.GetService<ICardService>();
            _accounts = fixture.GetService<IAccountService>();
            _customers = fixture.GetService<ICustomerService>();
        }

        [Fact]
        public void Card_Register_Success()
        {
            var customer = new Customer() {
                Firstname = "Dimitris",
                Lastname = "Pnevmatikos",
                VatNumber = "117008855",
                Email = "dpnevmatikos@codehub.gr",
                IsActive = true
            };

            var account = new Account() {
                Balance = 1000M,
                CurrencyCode = "EUR",
                State = Constants.AccountState.Active,
                AccountId = "GR123456789121"
            };

            customer.Accounts.Add(account);
            var cardNumber = GenerateCardNumber();

            var card = new Card() {
                Active = true,
                CardNumber = cardNumber,
                CardType = Constants.CardType.Debit
            };

            account.Cards.Add(card);

            _dbContext.Add(customer);
            _dbContext.SaveChanges();

            var customerFromDb = _dbContext.Set<Customer>()
                .Where(c => c.VatNumber == "117008855")
                .Include(c => c.Accounts)
                .ThenInclude(a => a.Cards)
                .SingleOrDefault();

            var customerCard = customerFromDb.Accounts
                .SelectMany(a => a.Cards)
                .Where(c => c.CardNumber == cardNumber)
                .SingleOrDefault();

            Assert.NotNull(customerCard);
            Assert.Equal(Constants.CardType.Debit, customerCard.CardType);
            Assert.True(customerCard.Active);
        }

        [Fact]
        public void Register_New_Card_On_Existing_Account_Success()
        {
            // create a new card
            var cardNumber = GenerateCardNumber();
            var cardResult = _cards.Register(new Services.Options.CreateCardOptions() {
                Active = true,
                CardNumber = cardNumber,
                CardType = CardType.Credit
            });
            Assert.Equal(ApiResultCode.Success, cardResult.Code);

            // verify card is in db
            cardResult = _cards.GetCardbyNumber(cardNumber);
            Assert.Equal(ApiResultCode.Success, cardResult.Code);

            // get ref to card
            var card = cardResult.Data;

            // get ref to specific account
            var accountResult = _accounts.GetById("CY00000000001248447601");
            Assert.Equal(ApiResultCode.Success, accountResult.Code);
            var account = accountResult.Data;

            // add card to account
            card.Accounts.Add(account);
            account.Cards.Add(card);

            _dbContext.SaveChanges();

            cardResult = _cards.GetCardbyNumber(cardNumber);
            Assert.Equal(ApiResultCode.Success, cardResult.Code);
        }

        [Theory]
        [InlineData("7229291478892443", "CY00000000001248447601")]
        public void Register_Existing_Card_To_Existing_Account_Success(string cardNumber, string accountNumber)
        {
            // get ref to card
            var cardResult = _cards.GetCardbyNumber(cardNumber);
            Assert.Equal(ApiResultCode.Success, cardResult.Code);
            var card = cardResult.Data;

            // get ref to account
            var accountResult = _accounts.GetById(accountNumber);
            Assert.Equal(ApiResultCode.Success, accountResult.Code);
            var account = accountResult.Data;

            // combine account to card and vice versa
            account.Cards.Add(card);
            card.Accounts.Add(account);

            // update database
            _dbContext.SaveChanges();
        }

        [Theory]
        [InlineData("3311731166885096", "CY00000000001035628576")]
        public void Register_Existing_Card_To_Existing_Account_Using_The_Card_Service_Method_Success(string cardNumber, string accountNumber)
        {
            // get ref to card
            var result = _cards.BindCardToAccount(cardNumber, accountNumber);
            Assert.Equal(ApiResultCode.Success, result.Code);
        }

        [Fact]
        public void Check_Card_Success()
        {
            SearchCardCheckoutOption option = new SearchCardCheckoutOption {
                Amount = 20,
                CardNumber = "7229291478892443",
                ExpirationMonth = 04,
                ExpirationYear = 2027
            };

            var cardResult = _cards.Checkout(option);
            Assert.Equal(ApiResultCode.Success, cardResult.Code);
        }
        private string GenerateCardNumber()
        {
            var cardNumber = "";

            for (byte i = 0; i < 4; i++) {
                cardNumber += $"{new Random().Next(1000, 9000)}";
            }

            return cardNumber;
        }
    }
}
