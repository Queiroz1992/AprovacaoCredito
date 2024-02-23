using AprovacaoCredito.Models;
using AprovacaoCredito.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AprovacaoCredito.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditoController : ControllerBase
    {
        private readonly ICreditService _creditService;
        private readonly ILogger<CreditoController> _logger;
        public CreditoController(ILogger<CreditoController> logger,ICreditService creditService)
        {
            _creditService = creditService;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<CreditResult> Post([FromBody] CreditRequest request)
        {
            var result = _creditService.ProcessCredit(request);
            
            if (result.IsApproved)
                return Ok(new { Status = "Aprovado", ValorTotalComJuros = result.TotalAmountWithInterest, ValorJuros = result.InterestAmount });
            else
                return BadRequest($"Crédito recusado de acordo com as premissas.{result.ReasonNotApproving}");            
        }
    }
}
