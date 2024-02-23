namespace AprovacaoCredito.Models
{
    public class CreditResult
    {
        public bool IsApproved { get; set; }
        public decimal TotalAmountWithInterest { get; set; }
        public decimal InterestAmount { get; set; }
        public string ReasonNotApproving { get; set; }
    }
}
