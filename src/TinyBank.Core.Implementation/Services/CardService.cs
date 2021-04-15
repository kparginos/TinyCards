﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Constants;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class CardService : ICardService
    {
        private readonly TinyBankDbContext _dbContext;

        public CardService(TinyBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApiResult<Card> BindCardToAccount(string cardNumber, string accountNumber)
        {
            // Basic Validations
            if (string.IsNullOrWhiteSpace(cardNumber)) {
                return ApiResult<Card>.UpdateFailed(
                    Constants.ApiResultCode.BadRequest, "Card Number cannot be empty");

            }

            if (string.IsNullOrWhiteSpace(accountNumber)) {
                return ApiResult<Card>.UpdateFailed(
                    Constants.ApiResultCode.BadRequest, "Account Number cannot be empty");
            }

            // get ref to card
            var cardResult = GetCardbyNumber(cardNumber);
            if (!cardResult.IsSuccessful()) {
                return cardResult;
            }
            var card = cardResult.Data;

            // get ref to account
            var account = _dbContext.Set<Account>()
                .Where(a => a.AccountId == accountNumber)
                .SingleOrDefault();

            if (account == null) {
                return ApiResult<Card>.UpdateFailed(
                    Constants.ApiResultCode.BadRequest, "Account Number does not exist");
            };

            // combine account to card and vice versa
            account.Cards.Add(card);
            card.Accounts.Add(account);

            // update database
            try {
                _dbContext.SaveChanges();
            }
            catch (Exception ex) {
                return ApiResult<Card>.UpdateFailed(
                    ApiResultCode.InternalServerError, $"Failed to update database: Details: {ex.Message}");
            }

            return new ApiResult<Card>() {
                Data = card
            };
        }

        public ApiResult<Card> GetCardbyNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber)) {
                return new ApiResult<Card>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = "Card Number cannot be empty !"
                };
            }

            var card = Search(
                new SearchCardOption() {
                    CardNumber = cardNumber
                })
                .Include(a => a.Accounts)
                .SingleOrDefault();

            if (card == null) {
                return new ApiResult<Card>() {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Card number {cardNumber} not found"
                };
            }

            return new ApiResult<Card>() {
                Code = ApiResultCode.Success,
                ErrorText = "Card information found",
                Data = card
            };
        }

        public ApiResult<Card> Register(CreateCardOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.CardNumber)) {
                return new ApiResult<Card>() {
                    Code = ApiResultCode.BadRequest,
                    ErrorText = "Card Number cannot be empty !"
                };
            }

            var card = new Card() {
                CardNumber = options.CardNumber,
                Active = options.Active,
                CardType = options.CardType,
                Expiration = options.Expiration
                //AvailableBalance = options.AvailableBalance,
            };

            try {
                _dbContext.Add<Card>(card);
                _dbContext.SaveChanges();
            }
            catch (Exception ex) {
                return new ApiResult<Card>() {
                    Code = ApiResultCode.InternalServerError,
                    ErrorText = $"Failed to save card. Details: {ex.Message}",
                };
            }

            return new ApiResult<Card>() {
                Code = ApiResultCode.Success,
                ErrorText = "Card saved.",
                Data = card
            };
        }

        public IQueryable<Card> Search(SearchCardOption options)
        {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }

            var q = _dbContext.Set<Card>()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(options.CardNumber)) {
                q = q.Where(c => c.CardNumber == options.CardNumber);
            }

            if (options.TrackResults != null &&
              !options.TrackResults.Value) {
                q = q.AsNoTracking();
            }

            if (options.Skip != null) {
                q = q.Skip(options.Skip.Value);
            }

            q = q.Take(options.MaxResults ?? 500);

            return q;
        }
    }
}
