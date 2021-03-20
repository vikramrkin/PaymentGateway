namespace Dto
{
    public class AuthorizeRequest
    {
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public string Cvv { get; set; }

        public double Amount { get; set; }
        public string Currency { get; set; }
    }
}
