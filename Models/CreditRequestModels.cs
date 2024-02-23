using System;

namespace AprovacaoCredito.Models
{
    public class CreditRequest
    {
        public decimal Amount { get; set; }
        public EnumCreditType CreditType { get; set; }
        public int Installments { get; set; }
        public DateTime FirstDueDate { get; set; }
    }
}
