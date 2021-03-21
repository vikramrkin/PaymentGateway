using System;
using Dto;
using Repository.Repo;

namespace Services
{
    public class RefundService : IRefundService
    {
        private readonly IPaymentGatewayRepo _repo;

        public RefundService(IPaymentGatewayRepo repo)
        {
            _repo = repo;
        }

        public RefundResponse Refund(RefundRequest refundRequest)
        {
            RefundResponse respose;

            if (_repo.TryGetAuthorization(refundRequest.AuthorizationId, out var authorization))
            {
                if (authorization.IsVoid)
                {
                    respose = new RefundResponse(authorization.Currency, authorization.AmountAuthorized) { IsError = true, Message = $"{authorization.CardNumber} - Refund failed as transaction is void"};
                    return respose;
                }
                
                if (refundRequest.Amount < authorization.AmountAuthorized)
                {
                    _repo.UpdateRefund(refundRequest.AuthorizationId, refundRequest.Amount);
                    respose = new RefundResponse(authorization.Currency, refundRequest.Amount) { Message = $"{authorization.CardNumber} - Successfully processed refund for {authorization.Currency} {refundRequest.Amount}"};
                }
                else
                {
                    respose = new RefundResponse(authorization.Currency, refundRequest.Amount) {  IsError = true, Message = $"{authorization.CardNumber} - Failed to process refund as refund amount is higher than total requested amount"};
                }

                return respose;
            }
            else
            {
                respose = new RefundResponse(string.Empty, refundRequest.Amount) { IsError = true, Message = $"Refund failed. Invalid authorization Id: {refundRequest.AuthorizationId}" };
                return respose;
            }
        }
    }

    public interface IRefundService
    {
        RefundResponse Refund(RefundRequest refundRequest);
    }
}
