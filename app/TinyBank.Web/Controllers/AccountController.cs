using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Web.Extensions;

namespace TinyBank.Web.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _account;
        public AccountController(IAccountService account)
        {
            _account = account;
        }

        [HttpPut("{accountId}")]
        public IActionResult Update(string accountId,
            [FromBody] UpdateAccountOptions options)
        {
            var result = _account.Update(accountId, options);

            if (!result.IsSuccessful()) {
                return result.ToActionResult();
            }

            return Ok();
        }

        [HttpGet("{accountId}")]
        public IActionResult Index(string accountId)
        {
            var result = _account.GetById(accountId);

            return Json(result);
        }
    }
}
