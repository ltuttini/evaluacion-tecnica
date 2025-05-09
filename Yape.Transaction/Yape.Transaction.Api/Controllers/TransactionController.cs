using Microsoft.AspNetCore.Mvc;
using MediatR;
using Yape.FinancialTransaction.Handles;

namespace Yape.FinancialTransaction.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Transaction([FromBody] CrearTransactionCommand comando)
        {
            await _mediator.Send(comando);
            return Ok();
        }

        [HttpPost("changestate")]
        public async Task<IActionResult> ChangeState([FromBody] ChangeStateCommand comando)
        {
            await _mediator.Send(comando);
            return Ok();
        }

    }
}
