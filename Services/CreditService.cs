using AprovacaoCredito.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AprovacaoCredito.Services
{
    public class CreditService : ICreditService
    {
        private CreditResult ValidarEntradas(CreditRequest request)
        {
            var result = new CreditResult();
            result.IsApproved = true;
            
            if (request.Amount <= 0 || request.Amount > 1000000)
            {
                result.IsApproved = false;
                result.ReasonNotApproving = "Valor do crédito inválido!";
                return result;
            }
            
            if (request.Installments < 5 || request.Installments > 72)
            {
                result.IsApproved = false;
                result.ReasonNotApproving = "Quantidade de parcelas inválida que é entre 5 e 72!";
                return result;
            }

            if (request.CreditType == EnumCreditType.PessoaJuridica && request.Amount < 15000)
            {
                result.IsApproved = false;
                result.ReasonNotApproving = "Valor do crédito abaixo do mínimo para pessoa jurídica que é de R$ 15.000,00!";
                return result;
            }

            var dataMinimaVencimento = DateTime.Today.AddDays(15);
            var dataMaximaVencimento = DateTime.Today.AddDays(40);

            if (request.FirstDueDate < dataMinimaVencimento || request.FirstDueDate > dataMaximaVencimento)
            {
                result.IsApproved = false;
                result.ReasonNotApproving = "Data do primeiro vencimento inválida!";
                return result;
            }

            return result;
        }
        public CreditResult ProcessCredit(CreditRequest request)
        {
            //Inicializar o resultado do processo de crédito
            var result = ValidarEntradas(request);

            if (!result.IsApproved)
                return result;

            // Calcular a taxa de juros com base no tipo de crédito
            decimal taxaJuros = 0;
            switch (request.CreditType)
            {
                case EnumCreditType.Direto:
                    taxaJuros = 0.02m; // 2%
                    break;
                case EnumCreditType.Consignado:
                    taxaJuros = 0.01m; // 1%
                    break;
                case EnumCreditType.PessoaJuridica:
                    taxaJuros = 0.05m; // 5%
                    break;
                case EnumCreditType.PessoaFisica:
                    taxaJuros = 0.03m; // 3%
                    break;
                case EnumCreditType.Imobiliario:
                    taxaJuros = 0.09m; // 9%
                    break;
            }

            // Calcular o valor total com juros e o valor dos juros
            decimal valorTotalComJuros = request.Amount * (1 + taxaJuros);
            decimal valorJuros = valorTotalComJuros - request.Amount;

            
            result.IsApproved = true;
            result.TotalAmountWithInterest = valorTotalComJuros;
            result.InterestAmount = valorJuros;

            return result;
        }
    }
}
