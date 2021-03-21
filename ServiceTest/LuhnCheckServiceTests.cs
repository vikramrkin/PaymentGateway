using NUnit.Framework;
using Services;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace ServiceTest
{
    public class LuhnCheckServiceTests
    {
        private ILuhnCheckService _luhnCheckService;
        
        [Xunit.Theory]
        [InlineData("79927398713")]
        [InlineData("5105105105105100")]
        [InlineData("4222222222222")]
        [InlineData("378282246310005")]
        [InlineData("378734493671000")]
        [InlineData("3530111333300000")]
        [InlineData("6331101999990016")]
        [InlineData("5610591081018250")]
        public void ValidCardNumberReturnsTrue(string cardNumber)
        {
            //Arrange
            _luhnCheckService = new LuhnCheckService();

            //Act
            var isValid = _luhnCheckService.IsValidCardNumber(cardNumber);

            //Assert
            Assert.IsTrue(isValid);
        }


        [Fact]
        public void TrailingSpaceIsIgnored()
        {
            //Arrange
            _luhnCheckService = new LuhnCheckService();

            //Act
            var isValid = _luhnCheckService.IsValidCardNumber("5105105105105100 ");

            //Assert
            Assert.IsTrue(isValid);
        }

        [Fact]
        public void LeadingSpaceIsIgnored()
        {
            //Arrange
            _luhnCheckService = new LuhnCheckService();

            //Act
            var isValid = _luhnCheckService.IsValidCardNumber(" 5105105105105100");

            //Assert
            Assert.IsTrue(isValid);
        }

        [Fact]
        public void SpaceInBetweenIsIgnored()
        {
            //Arrange
            _luhnCheckService = new LuhnCheckService();

            //Act
            var isValid = _luhnCheckService.IsValidCardNumber("5105 1051 0510 5100 ");

            //Assert
            Assert.IsTrue(isValid);
        }

        [Fact]
        public void HyphenIsIgnored()
        {
            //Arrange
            _luhnCheckService = new LuhnCheckService();

            //Act
            var isValid = _luhnCheckService.IsValidCardNumber("5105-1051-0510-5100 ");

            //Assert
            Assert.IsTrue(isValid);
        }

        [Fact]
        public void EmptyStringReturnsFalse()
        {
            //Arrange
            _luhnCheckService = new LuhnCheckService();

            //Act
            var isValid = _luhnCheckService.IsValidCardNumber("");

            //Assert
            Assert.IsFalse(isValid);
        }

        [Xunit.Theory]
        [InlineData("123")]
        [InlineData("5105xab")]
        [InlineData("42222222222225254")]
        [InlineData("3")]
        public void InvalidCardNumberReturnsFalse(string cardNumber)
        {
            //Arrange
            _luhnCheckService = new LuhnCheckService();

            //Act
            var isValid = _luhnCheckService.IsValidCardNumber(cardNumber);

            //Assert
            Assert.IsFalse(isValid);
        }
    }
}
