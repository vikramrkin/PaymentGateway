namespace Dto
{
    public class ResponseBase
    {
        public ResponseBase(string currency, double amountAvailable)
        {
            Currency = currency;
            AmountAvailable = amountAvailable;
        }

        public string Message { get; set; }
        public bool IsError { get; set; }
        public string Currency { get; }
        public double AmountAvailable { get; }
    }
}
