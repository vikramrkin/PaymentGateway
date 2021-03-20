namespace Repository.Entity
{
    public class Authorization
    {
        public string Id { get; set; }
        public string Currency { get; set; }
        public double AmountRequested { get; set; }
        public double AmountCaptured { get; set; }
        public bool IsRefunded { get; set; }
        public string CardNumber { get; set; }
    }
}
