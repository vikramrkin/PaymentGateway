namespace Dto
{
    public class CaptureResponse : ResponseBase
    {
        public CaptureResponse(string currency, double amount) : base(currency, amount)
        {
            
        }
    }
}