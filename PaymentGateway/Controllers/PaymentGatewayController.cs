using System;
using Dto;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace PaymentGateway.Controllers
{

    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;
        private readonly ICaptureService _captureService;
        private readonly IVoidService _voidService;
        private readonly IRefundService _refundService;

        public PaymentGatewayController(IAuthorizeService authorizeService, ICaptureService captureService, IVoidService voidService, IRefundService refundService)
        {
            _authorizeService = authorizeService;
            _captureService = captureService;
            _voidService = voidService;
            _refundService = refundService;
        }

        [Route("authorize")]
        [HttpPost]
        public AuthorizeResponse Authorize(AuthorizeRequest authRequest)
        {
            var response = _authorizeService.AuthorizeTransaction(authRequest);
            return response;
        }

        [Route("capture")]
        [HttpPut]
        public CaptureResponse Capture(CaptureRequest captureRequest)
        {
            var res = _captureService.Capture(captureRequest);
            return res;
        }

        [Route("refund")]
        [HttpPut]
        public RefundResponse Refund(RefundRequest refundRequest)
        {
            var response = _refundService.Refund(refundRequest);
            return response;
        }

        [Route("void")]
        [HttpPut]
        public VoidResponse Void(string authorizationId)
        {
            var response = _voidService.VoidTransaction(authorizationId);
            return response;
        }
    }
}
