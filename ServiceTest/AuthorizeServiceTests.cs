using Dto;
using Moq;
using NUnit.Framework;
using Repository.Entity;
using Repository.Repo;
using Services;

namespace ServiceTest
{
    public class Tests
    {
        private AuthorizeService _authService;
        private IPaymentGatewayRepo _repo;
        private Mock<IPaymentGatewayRepo> _mockRepo;
        private Mock<ILuhnCheckService> _mockLuhnCheckService;
        private AuthorizeRequest _authRequest;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPaymentGatewayRepo>();
            _mockLuhnCheckService = new Mock<ILuhnCheckService>();
            _repo = _mockRepo.Object;
            _authService = new AuthorizeService(_mockRepo.Object, _mockLuhnCheckService.Object);
        }

        [Test]
        public void AuthorizeReturnsUniqueId()
        {
            //Arrange
            _authRequest = new AuthorizeRequest() {CardNumber = "5105105105105100", Currency = "GBP", Amount = 20};
            var authId = "123-456";
            _mockLuhnCheckService.Setup(m => m.IsValidCardNumber(It.IsAny<string>())).Returns(true);
            _mockRepo.Setup(a => a.Authorize(It.IsAny<AuthorizeRequest>())).Returns(authId);

            //Act
            var authResponse = _authService.AuthorizeTransaction(_authRequest);

            //Assert
            Assert.AreEqual(authId, authResponse.AuthorizationId);
        }


        [Test]
        public void LuhnCheckFailResultsInError()
        {
            //Arrange
            _authRequest = new AuthorizeRequest() { CardNumber = "5105105105105100", Currency = "GBP", Amount = 20 };
            var authId = "123-456";
            _mockLuhnCheckService.Setup(m => m.IsValidCardNumber(It.IsAny<string>())).Returns(false);
            _mockRepo.Setup(a => a.Authorize(It.IsAny<AuthorizeRequest>())).Returns(authId);

            //Act
            var authResponse = _authService.AuthorizeTransaction(_authRequest);

            //Assert
            Assert.IsTrue(authResponse.IsError, "Received IsValid as true, expecting false");
            Assert.IsTrue(authResponse.Message.Contains("Received invalid credit card number"));
        }
    }
}