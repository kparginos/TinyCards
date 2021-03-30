using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customers;
        private readonly ILogger<HomeController> _logger;
        private readonly TinyBankDbContext _dbContext;

        // Path: '/customer'
        public CustomerController(
            TinyBankDbContext dbContext,
            ILogger<HomeController> logger,
            ICustomerService customers)
        {
            _logger = logger;
            _customers = customers;
            _dbContext = dbContext;
        }

        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            return Ok(new {
                id,
                endpoint = "get"
            });
        }

        [HttpPost]
        public IActionResult Register(
           [FromBody] RegisterCustomerOptions options)
        {
            return Ok(options);
        }
    }
}
