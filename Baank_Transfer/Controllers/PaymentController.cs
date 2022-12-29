using System.Threading.Tasks;
using Baank_Transfer.DTO_s.BankTransfer;
using Baank_Transfer.DTO_s.GenerateRecipient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Baank_Transfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpGet("ViewBanks")]
        public async Task<IActionResult> ViewBanks()
        {
            var banks = await _paymentService.ListOfBanks();
            if (banks.status == false) return BadRequest(banks);
            return Ok(banks);
        }
        [HttpPost("ValidateAccountNumber")]
        public async Task<IActionResult> ValidateAccountNumber([FromForm]string code, [FromForm]string accountNumber)
        {
            var response = await _paymentService.ValidateAccountNumber(code, accountNumber);
            if (response.status == false) return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("BankTransfer")]
        public async Task<IActionResult> BankTransfer([FromForm]BankTransferRequestModel model)
        {
            var response = await _paymentService.BankTranfer(model);
            if (response.status == false) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("GetTransaction/{transferId}")]
        public async Task<IActionResult> GetTransaction([FromRoute]string transferId)
        {
            var response = await _paymentService.GetTransaction(transferId);
            if (response.status == false) return BadRequest(response);
            return Ok(response);
        }
    }
}
