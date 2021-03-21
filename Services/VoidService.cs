using System.Net.NetworkInformation;
using Dto;
using Repository.Repo;

namespace Services
{
    public class VoidService : IVoidService
    {
        private readonly IPaymentGatewayRepo _repo;

        public VoidService(IPaymentGatewayRepo _repo)
        {
            this._repo = _repo;
        }

        public VoidResponse VoidTransaction(string authorizationId)
        {
            VoidResponse response;

            if (_repo.TryGetAuthorization(authorizationId, out var authorization))
            {
                if (authorization.IsVoid)
                {
                    response = new VoidResponse(authorization.Currency, authorization.AmountCaptured) {  IsError = true, Message = $"{authorization.CardNumber}: Unable to void transaction as it is already void"};
                }
                else
                {
                    _repo.VoidTransaction(authorizationId);
                    response = new VoidResponse(authorization.Currency, authorization.AmountCaptured) { Message = $"{authorization.CardNumber}: Transaction voided successfully"};
                }
            }
            else
            {
                response = new VoidResponse(string.Empty, -1) {  IsError = true, Message = $"Invalid authorization Id - {authorizationId}"};
            }

            return response;
        }
    }

    public interface IVoidService
    {
        VoidResponse VoidTransaction(string authorizationId);
    }
}