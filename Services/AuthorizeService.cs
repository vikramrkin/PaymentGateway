using Dto;
using Repository.Repo;

namespace Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IAuthorizationRepo _authRepo;

        public AuthorizeService(IAuthorizationRepo authRepo)
        {
            _authRepo = authRepo;
        }

        public AuthorizeResponse AuthorizeTransaction(AuthorizeRequest authRequest)
        {
            var authId =  _authRepo.Authorize(authRequest);
            return new AuthorizeResponse
                {AuthorizationId = authId, Amount = authRequest.Amount, Currency = authRequest.Currency};
        }
    }

    public interface IAuthorizeService
    {
        AuthorizeResponse AuthorizeTransaction(AuthorizeRequest authRequest);
    }
}
