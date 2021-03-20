using Dto;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("capture")]
    public class CaptureController : ControllerBase
    {
        private readonly ICaptureService _captureService;
        
        public CaptureController(ICaptureService captureService)
        {
            _captureService = captureService;
        }

        [HttpPost]
        public CaptureResponse Capture(CaptureRequest captureRequest)
        {
            var res = _captureService.Capture(captureRequest);
            return res;
        }
    }

    
}
