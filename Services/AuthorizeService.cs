using Dto;
using Repository.Repo;

namespace Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IPaymentGatewayRepo _authRepo;

        public AuthorizeService(IPaymentGatewayRepo authRepo)
        {
            _authRepo = authRepo;
        }

        public AuthorizeResponse AuthorizeTransaction(AuthorizeRequest authRequest)
        {
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
