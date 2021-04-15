using System;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class AccountServiceTests : IClassFixture<TinyBankFixture>
    {
        private readonly IAccountService _accounts;
        private readonly CustomerServiceTests _customerTests;
        private readonly ICustomerService _customer;

        public AccountServiceTests(TinyBankFixture fixture)
        {
            _accounts = fixture.GetService<IAccountService>();
            _customer = fixture.GetService<ICustomerService>();
            _customerTests = new CustomerServiceTests(fixture);
        }

        [Fact]
        public void CreateAccount_Success()
        {
            var customer = _customerTests.RegisterCustomer_Success(
                Constants.Country.GreekCountryCode);

            Assert.NotNull(customer);

            var accountOptions = new CreateAccountOptions() {
                CurrencyCode = Constants.CurrencyCode.Euro,
                Description = "My test account"
            };

            var accountResult = _accounts.Create(
                customer.CustomerId, accountOptions);
            Assert.True(accountResult.IsSuccessful());

            var account = accountResult.Data;
            Assert.StartsWith(customer.CountryCode, account.AccountId);
            Assert.Equal(customer.CustomerId, account.CustomerId);
            Assert.Equal(0M, account.Balance);
            Assert.Equal(Constants.AccountState.Active, account.State);
        }

        [Fact]
        public void CreateAccountOnExistingCustomer_Success()
        {
            var resultCustomer = _customer.GetById(Guid.Parse("5294743F-2622-40F3-B662-31319F3BC041"));
            Assert.Equal(Constants.ApiResultCode.Success, resultCustomer.Code);

            var customer = resultCustomer.Data;

            var accountOptions = new CreateAccountOptions() {
                CurrencyCode = Constants.CurrencyCode.Euro,
                Description = "My test account"
            };

            var accountResult = _accounts.Create(
                customer.CustomerId, accountOptions);
            Assert.True(accountResult.IsSuccessful());

            var account = accountResult.Data;
            Assert.StartsWith(customer.CountryCode, account.AccountId);
            Assert.Equal(customer.CustomerId, account.CustomerId);
            Assert.Equal(0M, account.Balance);
            Assert.Equal(Constants.AccountState.Active, account.State);
        }

        [Fact]
        public void UpdateAccount_Success()
        {
            var resultCustomer = _customer.GetById(Guid.Parse("5294743F-2622-40F3-B662-31319F3BC041"));

            Assert.Equal(Constants.ApiResultCode.Success, resultCustomer.Code);
            Assert.NotEmpty(resultCustomer.Data.Accounts);

            var resultAccount = _accounts.Update(resultCustomer.Data.Accounts[1].AccountId, new UpdateAccountOptions() {
                Balance = 4000,
                CurrencyCode = "EUR",
                Description = "***** Some Account *****",
                State = Constants.AccountState.Suspended
            });

            Assert.Equal(Constants.ApiResultCode.Success, resultAccount.Code);
        }
    }
}
