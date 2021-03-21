namespace Repository.Entity
{
    public class Authorization
    {
        public Authorization(string cardNumber, string currency, double amountAuthorized)
        {
            CardNumber = cardNumber;
            Currency = currency;
            AmountAuthorized = amountAuthorized;
        }

        public string CardNumber { get; }
        public string Currency { get; }
        public double AmountAuthorized { get; }
        
        public double AmountCaptured { get; set; }
        public bool IsRefunded { get; set; }
        public bool IsVoid { get; set; }
    }
}
