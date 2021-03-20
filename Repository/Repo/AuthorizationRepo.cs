using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dto;
using Repository.Entity;
using Repository.Services;

namespace Repository.Repo
{ 
    public class AuthorizationRepo : IAuthorizationRepo
    {
        private readonly IUniqueIdGenerator _uniqueIdGenerator;
        private readonly IDictionary<string, Authorization> _authLookup;

        public AuthorizationRepo(IUniqueIdGenerator uniqueIdGenerator)
        {
            _uniqueIdGenerator = uniqueIdGenerator;
            _authLookup = new ConcurrentDictionary<string, Authorization>();
        }
        public string Authorize(AuthorizeRequest authRequest)
        {
            var uniqueId = _uniqueIdGenerator.GetUniqueId();
            _authLookup.Add(uniqueId, new Authorization
            {
                AmountRequested = authRequest.Amount,
                Currency = authRequest.Currency,
                CardNumber = authRequest.CardNumber
            });

            return uniqueId;
        }

        public string Capture(string authId, double amount)
        {
            if (!_authLookup.TryGetValue(authId, out var authorization))
            {
                throw new ApplicationException($"Received invalid authorization Id - {authId} for amount - {amount}");
            }

            var totalAmountCaptured = authorization.AmountCaptured + amount;

            if (totalAmountCaptured > authorization.AmountRequested)
            {
                throw new ApplicationException($"{authorization.CardNumber}: Authorization failed. Total amount captured is greater than authorized amount");
            }

            _authLookup[authId].AmountCaptured = totalAmountCaptured;

            return $"{authorization.CardNumber}: Capture successful";
        }
    }

    public interface IAuthorizationRepo
    {
        string Authorize(AuthorizeRequest authRequest);
        string Capture(string authId, double amount);
    }
}
