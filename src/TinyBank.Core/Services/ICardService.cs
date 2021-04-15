using System.Linq;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Services
{
    public interface ICardService
    {
        public ApiResult<Card> Register(CreateCardOptions options);
        public ApiResult<Card> BindCardToAccount(string cardNumber, string accontNumber);
        public ApiResult<Card> GetCardbyNumber(string cardNumber);
        public IQueryable<Card> Search(SearchCardOption options);
    }
}
