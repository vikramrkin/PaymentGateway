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
            RefundResponse response;

            if (_repo.TryGetAuthorization(refundRequest.AuthorizationId, out var authorization))
            {
                if (authorization.IsVoid)
                {
                    response = new RefundResponse(authorization.Currency, authorization.AmountAuthorized) { IsError = true, Message = $"{authorization.CardNumber} - Refund failed as transaction is void"};
                    return response;
                }
                
                if (refundRequest.Amount <= authorization.AmountAuthorized)
                {
                    _repo.UpdateRefund(refundRequest.AuthorizationId, refundRequest.Amount);
                    response = new RefundResponse(authorization.Currency, refundRequest.Amount) { Message = $"{authorization.CardNumber} - Successfully processed refund for {authorization.Currency} {refundRequest.Amount}"};
                }
                else
                {
                    response = new RefundResponse(authorization.Currency, refundRequest.Amount) {  IsError = true, Message = $"{authorization.CardNumber} - Failed to process refund as refund amount is higher than total requested amount"};
                }

                return response;
            }
            else
            {
                response = new RefundResponse(string.Empty, refundRequest.Amount) { IsError = true, Message = $"Refund failed. Invalid authorization Id: {refundRequest.AuthorizationId}" };
                return response;
            }
        }
    }

    public interface IRefundService
    {
        RefundResponse Refund(RefundRequest refundRequest);
    }
}
