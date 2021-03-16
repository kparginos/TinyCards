using System;
using System.Linq;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TinyBankDbContext _dbContext;

        public Customer Register(RegisterCustomerOptions options)
        {
            if (options == null) {
                return null;
            }

            if (string.IsNullOrWhiteSpace(options.Firstname)) {
                return null;
            }

            if (string.IsNullOrWhiteSpace(options.Lastname)) {
                return null;
            }

            if (options.Type == Constants.CustomerType.Undefined) {
                return null;
            }

            if (!Constants.Country.SupportedCountryCodes.Contains(
              options.CountryCode)) {
                return null;
            }
        }
    }
}
