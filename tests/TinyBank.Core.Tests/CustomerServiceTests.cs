using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Implementation.Services;
using TinyBank.Core.Services;

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
        public void RegisterCustomer_Success()
        {
            var result = _customers.Register(
                new Services.Options.RegisterCustomerOptions() {
                    Firstname = "Dimitris",
                    Lastname = "Pnevmatikos",
                    Type = Constants.CustomerType.PhysicalEntity,
                    CountryCode = Constants.Country.GreekCountryCode,
                    VatNumber = "1111111"
                });
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
