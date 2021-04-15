using System;
using System.Linq;

using TinyBank.Core.Model;

namespace TinyBank.Core.Services
{
    public interface IAccountService
    {
        public ApiResult<Account> Create(Guid customerId,
            Options.CreateAccountOptions options);
        public ApiResult<Account> Update(string accountId,
            Options.UpdateAccountOptions options);
        public ApiResult<Account> GetById(string accountId);
        public IQueryable<Account> Search(
            Options.SearchAccountOptions options);
    }
}
