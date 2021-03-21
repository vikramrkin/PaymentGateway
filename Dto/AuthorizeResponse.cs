namespace Dto
{
    public class AuthorizeResponse : ResponseBase
    {
        public AuthorizeResponse(string authorizationId, string currency, double amountAvailable) : base(currency, amountAvailable)
        {
            AuthorizationId = authorizationId;
        }

        public string AuthorizationId { get; }
    }
}
