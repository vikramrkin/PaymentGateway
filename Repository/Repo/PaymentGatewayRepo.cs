using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Dto;
using Repository.Entity;
using Repository.Services;

namespace Repository.Repo
{ 
    public class PaymentGatewayRepo : IPaymentGatewayRepo
    {
        private readonly IUniqueIdGenerator _uniqueIdGenerator;
        private readonly IDictionary<string, Authorization> _authLookup;

        public PaymentGatewayRepo(IUniqueIdGenerator uniqueIdGenerator)
        {
            _uniqueIdGenerator = uniqueIdGenerator;
            _authLookup = new ConcurrentDictionary<string, Authorization>();
        }

        public bool TryGetAuthorization(string authorizationId, out Authorization authorization)
        {
            return _authLookup.TryGetValue(authorizationId, out authorization);
        }

        public string Authorize(AuthorizeRequest authRequest)
        {
            var uniqueId = _uniqueIdGenerator.GetUniqueId();
            _authLookup.Add(uniqueId, new Authorization(authRequest.CardNumber, authRequest.Currency, authRequest.Amount));

            return uniqueId;
        }


        public void VoidTransaction(string authorizationId)
        {
            if (!_authLookup.TryGetValue(authorizationId, out var authorization))
            {
                throw new ApplicationException($"Received invalid authorization Id - {authorizationId}");
            }

            authorization.IsVoid = true;
            authorization.AmountCaptured = 0;
        }

        public void UpdateCapture(string authorizationId, double amount)
        {
            if (!_authLookup.TryGetValue(authorizationId, out var authorization))
            {
                throw new ApplicationException($"Received invalid authorization Id - {authorizationId} for amount - {amount}");
            }

            var totalAmountCaptured = authorization.AmountCaptured + amount;

            if (totalAmountCaptured > authorization.AmountAuthorized)
            {
                throw new ApplicationException($"{authorization.CardNumber}: Capture failed. Total amount captured is greater than authorized amount");
            }

            authorization.AmountCaptured = totalAmountCaptured;
        }

        public void UpdateRefund(string authorizationId, double amount)
        {
            if (!_authLookup.TryGetValue(authorizationId, out var authorization))
            {
                throw new ApplicationException($"Received invalid authorization Id - {authorizationId} for amount - {amount}");
            }

            if (amount > authorization.AmountAuthorized)
            {
                throw new ApplicationException($"{authorization.CardNumber} - Refund failed. Refund requested is greater than total amount requested");
            }

            authorization.AmountCaptured -= amount;
            authorization.IsRefunded = true;
        }
    }

    public interface IPaymentGatewayRepo
    {
        bool TryGetAuthorization(string authorizationId, out Authorization authorization);
        string Authorize(AuthorizeRequest authRequest);
        void UpdateCapture(string authorizationId, double amount);
        void UpdateRefund(string authorizationId, double amount);
        void VoidTransaction(string authorizationId);
    }
}
