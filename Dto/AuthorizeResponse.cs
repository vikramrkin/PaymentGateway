namespace Dto
{
    public class AuthorizeResponse
    {
        public string AuthorizationId { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }
    }
}
