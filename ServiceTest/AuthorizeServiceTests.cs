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
        private Authorization _authorizationEntity;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPaymentGatewayRepo>();
            _repo = _mockRepo.Object;
            _authService = new AuthorizeService(_mockRepo.Object);
        }

        [Test]
        public void AuthorizeReturnsUniqueId()
        {
            //Arrange
            var authId = "123-456";
            _authorizationEntity = new Authorization("1234-5678-1234-5678", "GBP", 20);
            _mockRepo.Setup(a => a.Authorize(It.IsAny<AuthorizeRequest>())).Returns(authId);

            //Act
            var actualAuthId = _repo.Authorize(new AuthorizeRequest());

            //Assert
            Assert.AreEqual(authId, actualAuthId);
        }
    }
}