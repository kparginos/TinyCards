using Microsoft.AspNetCore.Mvc;

using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    [Route("card")]
    public class CardController : Controller
    {
        private readonly ICardService _card;

        public CardController(ICardService card)
        {
            _card = card;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout([FromBody] SearchCardCheckoutOption options)
        {
            //var result = _card.
            return View();
        }

        [HttpGet("{cardNumber}")]
        public IActionResult GetCard(string cardNumber)
        {
            var result = _card.GetCardbyNumber(cardNumber);

            return Json(result);
        }
    }
}
