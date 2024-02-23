using AprovacaoCredito.Models;

namespace AprovacaoCredito.Services
{
    public interface ICreditService
    {
        CreditResult ProcessCredit(CreditRequest request);
    }
}
