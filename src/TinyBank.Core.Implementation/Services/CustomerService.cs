using AutoMapper;

using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public CustomerService(TinyBankDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<RegisterCustomerOptions, Customer>())
                .CreateMapper();
        }

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

            if (!IsValidVatNumber(options.CountryCode, options.VatNumber)) {
                return null;
            }

            var customer = _mapper.Map<Customer>(options);
            customer.IsActive = true;

            _dbContext.Add(customer);

            try {
                _dbContext.SaveChanges();
            } catch(Exception ex) {
                // log
                Console.WriteLine(ex);

                return null;
            }

            return customer;
        }

        public bool IsValidVatNumber(
            string countryCode, string vatNumber)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) {
                return false;
            }

            if (string.IsNullOrWhiteSpace(vatNumber)) {
                return false;
            }

            if (!Constants.Country.VatLength.TryGetValue(
              countryCode, out var vatLength)) {
                return false;
            }

            return vatNumber.Length == vatLength;
        }
    }
}
