using Dto;
using Repository.Repo;

namespace Services
{
    public class CaptureService : ICaptureService
    {
        private readonly IAuthorizationRepo _repo;

        public CaptureService(IAuthorizationRepo repo)
        {
            _repo = repo;
        }
        public CaptureResponse Capture(CaptureRequest captureRequest)
        {
            var response = _repo.Capture(captureRequest.AuthorizationId, captureRequest.Amount);
            return new CaptureResponse
            {
                Amount = captureRequest.Amount, StatusMessage = response
            };
        }
    }

    public interface ICaptureService
    {
        CaptureResponse Capture(CaptureRequest captureRequest);
    }
}