using Dto;
using Repository.Entity;
using Repository.Repo;

namespace Services
{
    public class CaptureService : ICaptureService
    {
        private readonly IPaymentGatewayRepo _repo;

        public CaptureService(IPaymentGatewayRepo repo)
        {
            _repo = repo;
        }
        public CaptureResponse Capture(CaptureRequest captureRequest)
        {
            CaptureResponse respose;

            if (_repo.TryGetAuthorization(captureRequest.AuthorizationId, out var authorization))
            {
                if (IsCaptureAllowed(authorization))
                {
                    if (authorization.AmountCaptured + captureRequest.Amount < authorization.AmountAuthorized)
                    {
                        _repo.UpdateCapture(captureRequest.AuthorizationId, captureRequest.Amount);
                        respose = new CaptureResponse(authorization.Currency, captureRequest.Amount) { Message = $"{authorization.CardNumber} - Successfully captured {authorization.Currency} {captureRequest.Amount}" };
                    }
                    else
                    {
                        respose = new CaptureResponse(authorization.Currency, captureRequest.Amount) { IsError = true, Message = $"{authorization.CardNumber} - Failed to capture {authorization.Currency} {captureRequest.Amount} as it exceeds authorized amount" };
                    }
                }
                else
                {
                    respose = new CaptureResponse(authorization.Currency, captureRequest.Amount) {  IsError = true, Message = $"{authorization.CardNumber} - Capture not allowed on a void/refunded transaction"};
                }

                return respose;
            }
            else
            {
                respose = new CaptureResponse(string.Empty, captureRequest.Amount)
                    {IsError = true, Message = $"Capture failed. Invalid authorization Id: {captureRequest.AuthorizationId}"};
                return respose;
            }
        }

        public bool IsCaptureAllowed(Authorization authorization)
        {
            var isCaptureAllowed = !(authorization.IsVoid || authorization.IsRefunded);
            return isCaptureAllowed;
        }
    }

    public interface ICaptureService
    {
        CaptureResponse Capture(CaptureRequest captureRequest);
        bool IsCaptureAllowed(Authorization authorization);
    }
}