namespace Dto
{
    public class VoidResponse : ResponseBase
    {
        public VoidResponse(string currency, double amountAvailable) : base(currency, amountAvailable)
        {
        }
    }
}
