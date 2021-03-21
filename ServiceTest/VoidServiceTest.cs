using Moq;
using NUnit.Framework;
using Repository.Entity;
using Repository.Repo;
using Services;

namespace ServiceTest
{
    public class VoidServiceTest
    {
        private IVoidService _voidService;
        private IPaymentGatewayRepo _repo;
        private Mock<IPaymentGatewayRepo> _mockRepo;
        private Authorization _authorizationEntity;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPaymentGatewayRepo>();
            _repo = _mockRepo.Object;
            _voidService = new VoidService(_repo);
        }

        [Test]
        public void ErrorWhenAuthorizationIdNotFound()
        {
            //Arrange
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(false);
            var authorizationId = "12345678";

            //Act
            var response = _voidService.VoidTransaction(authorizationId);

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
            var authorizationId = "12345678";

            //Act
            var response = _voidService.VoidTransaction(authorizationId);

            //Assert
            Assert.IsTrue(response.Message.Contains("as it is already void"));
            Assert.IsTrue(response.IsError);
        }


        [Test]
        public void TransactionVoidsSuccessfully()
        {
            //Arrange
            _authorizationEntity = new Authorization("123", "GBP", 10);
            _mockRepo.Setup(m => m.TryGetAuthorization(It.IsAny<string>(), out _authorizationEntity)).Returns(true);
            var authorizationId = "12345678";

            //Act
            var response = _voidService.VoidTransaction(authorizationId);

            //Assert
            Assert.IsFalse(response.IsError);
        }
    }
}
