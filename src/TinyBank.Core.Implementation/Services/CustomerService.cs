using AutoMapper;

using System;
using System.Linq;

using TinyBank.Core.Constants;
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

        public ApiResult<Customer> Register(RegisterCustomerOptions options)
        {
            if (options == null) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null {nameof(options)}"
                };
            }

            if (string.IsNullOrWhiteSpace(options.Firstname)) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.Firstname)}"
                };
            }

            if (string.IsNullOrWhiteSpace(options.Lastname)) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Null or empty {nameof(options.Lastname)}"
                };
            }

            if (options.Type == Constants.CustomerType.Undefined) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Invalid customer type {nameof(options.Type)}"
                };
            }

            if (!IsValidVatNumber(options.CountryCode, options.VatNumber)) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = $"Invalid Vat number {options.VatNumber}"
                };
            }

            var customerExists = _dbContext.Set<Customer>()
                .Any(c => c.VatNumber == options.VatNumber);

            if (customerExists) {
                return new ApiResult<Customer>() {
                    Code = ApiResultCode.Conflict,
                    ErrorText = $"Customer with Vat number {options.VatNumber} already exists"
                };
            }

            var customer = _mapper.Map<Customer>(options);
            customer.IsActive = true;

            _dbContext.Add(customer);

            try {
                _dbContext.SaveChanges();
            } catch(Exception ex) {
                // log
                Console.WriteLine(ex);

                return new ApiResult<Customer>() {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Customer could not be saved"
                };
            }

            return new ApiResult<Customer>() {
                Data = customer
            };
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
