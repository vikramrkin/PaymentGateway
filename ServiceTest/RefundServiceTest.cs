using Dto;
using Moq;
using NUnit.Framework;
using Repository.Entity;
using Repository.Repo;
using Services;

namespace ServiceTest
{
    public class RefundServiceTest
    {
        private IRefundService _refundService;
        private IPaymentGatewayRepo _repo;
        private Mock<IPaymentGatewayRepo> _mockRepo;
        private Authorization _authorizationEntity;
        private RefundRequest _refundRequest;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPaymentGatewayRepo>();
            _repo = _mockRepo.Object;
            _refundService = new RefundService(_repo);
        }

        [Test]
        public void ErrorWhenAuthorizationIdNotFound()
        {
            //Arrange
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(false);
            _refundRequest = new RefundRequest { AuthorizationId = "123" };

            //Act
            var response = _refundService.Refund(_refundRequest);

            //Assert
            Assert.IsTrue(response.Message.Contains("Invalid authorization Id"));
            Assert.IsTrue(response.IsError);
        }

        [Test]
        public void ErrorWhenVoid()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10) { IsVoid = true };
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _refundRequest = new RefundRequest { AuthorizationId = "123" };

            //Act
            var response = _refundService.Refund(_refundRequest);

            //Assert
            Assert.IsTrue(response.Message.Contains("Refund failed as transaction is void"));
            Assert.IsTrue(response.IsError);
        }

        [Test]
        public void ErrorWhenRefundExceedsCaptured()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _refundRequest = new RefundRequest { AuthorizationId = "123", Amount = 12};

            //Act
            var response = _refundService.Refund(_refundRequest);

            //Assert
            Assert.IsTrue(response.Message.Contains("refund amount is higher than total requested amount"));
            Assert.IsTrue(response.IsError);
        }

        [Test]
        public void RefundReturnsTrueWhenSuccessful()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _refundRequest = new RefundRequest { AuthorizationId = "123", Amount = 5 };

            //Act
            var response = _refundService.Refund(_refundRequest);

            //Assert
            Assert.IsFalse(response.IsError);
        }

        [Test]
        public void RefundSuccessfulWhenAmountLessThanCaptured()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _refundRequest = new RefundRequest { AuthorizationId = "123", Amount = 4 };

            //Act
            var response = _refundService.Refund(_refundRequest);

            //Assert
            Assert.IsFalse(response.IsError);
        }

        [Test]
        public void RefundSuccessfulWhenAmountEqualToCaptured()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            _refundRequest = new RefundRequest { AuthorizationId = "123", Amount = 10 };

            //Act
            var response = _refundService.Refund(_refundRequest);

            //Assert
            Assert.IsFalse(response.IsError);
        }
    }
}
