using Dto;
using Moq;
using NUnit.Framework;
using Repository.Entity;
using Repository.Repo;
using Services;

namespace ServiceTest
{
    public class CaptureServiceTest
    {
        private ICaptureService _captureService;
        private IPaymentGatewayRepo _repo;
        private Mock<IPaymentGatewayRepo> _mockRepo;
        private Authorization _authorizationEntity;
        private CaptureRequest _captureRequest;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPaymentGatewayRepo>();
            _repo = _mockRepo.Object;
            _captureService = new CaptureService(_repo);
        }

        [Test]
        public void ErrorWhenAuthorizationIdNotFound()
        {
            //Arrange
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(false);
            _captureRequest = new CaptureRequest { AuthorizationId = "123"};

            //Act
            var response = _captureService.Capture(_captureRequest);

            //Assert
            Assert.IsTrue(response.Message.Contains("Invalid authorization Id"));
            Assert.IsTrue(response.IsError);
        }

        [Test]
        public void ErrorWhenVoid()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10) {IsVoid = true};
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123" };

            //Act
            var response = _captureService.Capture(_captureRequest);

            //Assert
            Assert.IsTrue(response.Message.Contains("Capture not allowed on a void/refunded transaction"));
            Assert.IsTrue(response.IsError);
        }

        [Test]
        public void ErrorWhenRefunded()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10) { IsRefunded = true };
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123" };

            //Act
            var response = _captureService.Capture(_captureRequest);

            //Assert
            Assert.IsTrue(response.Message.Contains("Capture not allowed on a void/refunded transaction"));
            Assert.IsTrue(response.IsError);
        }

        [Test]
        public void ErrorWhenTotalCapturedExceedsAuthorized()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123", Amount = 100};

            //Act
            var response = _captureService.Capture(_captureRequest);

            //Assert
            Assert.IsTrue(response.Message.Contains("exceeds authorized amount"));
            Assert.IsTrue(response.IsError);
        }


        [Test]
        public void CaptureReturnsTrueWhenSuccessful()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123", Amount = 5 };

            //Act
            var response = _captureService.Capture(_captureRequest);

            //Assert
            Assert.IsFalse(response.IsError);
        }


        [Test]
        public void CaptureSuccessfulWhenRequestedIsEqualToAuthorized()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123", Amount = 10 };

            //Act
            var response = _captureService.Capture(_captureRequest);

            //Assert
            Assert.IsFalse(response.IsError);
        }

        [Test]
        public void IsCaptureAllowedFalseWhenVoid()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10) { IsVoid = true};
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123", Amount = 10 };

            //Act
            var isCaptureAllowed = _captureService.IsCaptureAllowed(_authorizationEntity);

            //Assert
            Assert.IsFalse(isCaptureAllowed);
        }

        [Test]
        public void IsCaptureAllowedFalseWhenRefunded()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10) { IsRefunded = true };
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123", Amount = 10 };

            //Act
            var isCaptureAllowed = _captureService.IsCaptureAllowed(_authorizationEntity);

            //Assert
            Assert.IsFalse(isCaptureAllowed);
        }

        [Test]
        public void IsCaptureAllowedTrueWhenNotVoidAndNotRefunded()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _captureRequest = new CaptureRequest { AuthorizationId = "123", Amount = 10 };

            //Act
            var isCaptureAllowed = _captureService.IsCaptureAllowed(_authorizationEntity);

            //Assert
            Assert.IsTrue(isCaptureAllowed);
        }
    }
}
