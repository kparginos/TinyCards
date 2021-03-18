using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Implementation.Services;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class CustomerServiceTests : IClassFixture<TinyBankFixture>
    {
        private readonly TinyBankDbContext _dbContext;

        public CustomerServiceTests(TinyBankFixture fixture)
        {
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public void RegisterCustomer_Success()
        {
            var customerService = new CustomerService(
                _dbContext);

            var result = customerService.Register(
                new Services.Options.RegisterCustomerOptions() {
                    Firstname = "Dimitris",
                    Lastname = "Pnevmatikos",
                    Type = Constants.CustomerType.PhysicalEntity,
                    CountryCode = Constants.Country.GreekCountryCode,
                    VatNumber = "1111111"
                });
        }
    }
}
