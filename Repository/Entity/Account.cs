namespace Repository.Entity
{
    public class Account
    {
        public int Id { get; set; }
        public string CreditCardNumber { get; set; }
        public string Currency { get; set; }
        public double CreditLimit { get; set; }
        public double Balance { get; set; }
    }
}
