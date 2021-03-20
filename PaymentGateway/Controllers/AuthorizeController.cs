using Dto;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("authorize")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;

        public AuthorizeController(IAuthorizeService authorizeService)
        {
            _authorizeService = authorizeService;
        }

        [HttpPost]
        public AuthorizeResponse Authorize(AuthorizeRequest authRequest)
        {
            var response = _authorizeService.AuthorizeTransaction(authRequest);
            return response;
        }
    }
}
