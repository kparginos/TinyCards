using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Services;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerService _customers;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            ICustomerService customers)
        {
            _logger = logger;
            _customers = customers;
        }

        public IActionResult Index()
        {
            //return View();
            var customers = _customers.Search(
                new Core.Services.Options.SearchCustomerOptions() {
                    MaxResults = 10
                }).ToList();

            return Json(customers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
