namespace Dto
{
    public class RefundResponse : ResponseBase
    {
        public RefundResponse(string currency, double amountAvailable) : base(currency, amountAvailable)
        {

        }
    }
}