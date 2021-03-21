using Dto;
using Repository.Repo;

namespace Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IPaymentGatewayRepo _authRepo;
        private readonly ILuhnCheckService _luhnCheckService;

        public AuthorizeService(IPaymentGatewayRepo authRepo, ILuhnCheckService luhnCheckService)
        {
            _authRepo = authRepo;
            _luhnCheckService = luhnCheckService;
        }

        public AuthorizeResponse AuthorizeTransaction(AuthorizeRequest authRequest)
        {
            if (!_luhnCheckService.IsValidCardNumber(authRequest.CardNumber))
            {
                return new AuthorizeResponse("-1", string.Empty, -1) { IsError = true, Message = $"{authRequest.CardNumber}: Received invalid credit card number"};
            }

            var authId =  _authRepo.Authorize(authRequest);
            var response = new AuthorizeResponse(authId, authRequest.Currency, authRequest.Amount)
            {
                Message =
                    $"{authRequest.CardNumber}: Authorization successful for amount {authRequest.Currency} {authRequest.Amount}."
            };

            return response;
        }
    }

    public interface IAuthorizeService
    {
        AuthorizeResponse AuthorizeTransaction(AuthorizeRequest authRequest);
    }
}
