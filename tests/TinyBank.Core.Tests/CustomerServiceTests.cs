using System;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Implementation.Services;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class CustomerServiceTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;
        private readonly ICustomerService _customers;

        public CustomerServiceTests(TinyBankFixture fixture)
        {
            _dbContext = fixture.DbContext;
            _customers = new CustomerService(_dbContext);
        }

        [Fact]
        public Customer RegisterCustomer_Success()
        {
            var vatnumber = $"{DateTimeOffset.Now:ssfffffff}";

            var options = new RegisterCustomerOptions() {
                Firstname = "Dimitris",
                Lastname = "Pnevmatikos",
                Type = Constants.CustomerType.PhysicalEntity,
                CountryCode = Constants.Country.GreekCountryCode,
                VatNumber = vatnumber
            };

            var result = _customers.Register(options);

            Assert.True(result.IsSuccessful());
            Assert.NotNull(result.Data);

            var customer = result.Data;
            Assert.Equal(options.Firstname, customer.Firstname);
            Assert.Equal(options.Lastname, customer.Lastname);
            Assert.Equal(options.Type, customer.Type);
            Assert.Equal(options.VatNumber, customer.VatNumber);
            Assert.True(customer.IsActive);

            return customer;
        }

        [Fact]
        public void RegisterCustomer_Fail_Customer_Exists()
        {
            var customer = RegisterCustomer_Success();
            Assert.NotNull(customer);

            var options = new RegisterCustomerOptions() {
                CountryCode = customer.CountryCode,
                VatNumber = customer.VatNumber,
                Firstname = "Name",
                Lastname = "Lastname",
                Type = Constants.CustomerType.PhysicalEntity
            };

            var result = _customers.Register(options);
            Assert.False(result.IsSuccessful());
            Assert.Equal(Constants.ApiResultCode.Conflict, result.Code);
        }

        [Fact]
        public void RegisterCustomer_Fail_InvalidOptions()
        {
            var options = new RegisterCustomerOptions() {
                Lastname = "Pnevmatikos",
                Type = Constants.CustomerType.PhysicalEntity,
                CountryCode = Constants.Country.GreekCountryCode,
                VatNumber = "1111111"
            };

            // Firstname
            var result = _customers.Register(options);
            Assert.False(result.IsSuccessful());
            Assert.Equal(Constants.ApiResultCode.BadRequest, result.Code);

            // lastname
            options.Firstname = "Dimitris";
            options.Lastname = null;

            result = _customers.Register(options);
            Assert.False(result.IsSuccessful());
            Assert.Equal(Constants.ApiResultCode.BadRequest, result.Code);
        }

        [Fact]
        public void ValidateVatNumber()
        {
            // success - happy path
            var result = _customers.IsValidVatNumber(
                Constants.Country.GreekCountryCode, "123456789");
            Assert.True(result);

            result = _customers.IsValidVatNumber(
                Constants.Country.ItalyCountryCode, "1234567891");
            Assert.True(result);

            result = _customers.IsValidVatNumber(
                Constants.Country.CyprusCountryCode, "12345678911");
            Assert.True(result);

            // fail
            result = _customers.IsValidVatNumber(
                "GB", "123456789");
            Assert.False(result);

            result = _customers.IsValidVatNumber(
                Constants.Country.GreekCountryCode, "         ");
            Assert.False(result);

            result = _customers.IsValidVatNumber(
                "gR", "123456789");
            Assert.True(result);
        }
    }
}
