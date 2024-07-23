using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation.Results;
using Moq;

namespace EmpXpo.Accounting.Tests
{
    public class SellerValidatorServiceTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly SellerValidatorService _validatorService;

        public SellerValidatorServiceTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _validatorService = new SellerValidatorService(_notifierServiceMock.Object);
        }

        [Fact]
        public async Task IsValidAsync_ValidSeller_ReturnsTrue()
        {
            // Arrange
            var seller = new Seller
            {
                Name = "Valid Name",
                Email = "valid@example.com"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsValidAsync_InvalidName_ReturnsFalse()
        {
            // Arrange
            var seller = new Seller
            {
                Name = "",
                Email = "valid@example.com"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            var seller = new Seller
            {
                Name = "Valid Name",
                Email = "invalid-email"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_EmptyEmail_ReturnsFalse()
        {
            // Arrange
            var seller = new Seller
            {
                Name = "Valid Name",
                Email = ""
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_NullName_ReturnsFalse()
        {
            // Arrange
            var seller = new Seller
            {
                Name = null,
                Email = "valid@example.com"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_NullEmail_ReturnsFalse()
        {
            // Arrange
            var seller = new Seller
            {
                Name = "Valid Name",
                Email = null
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }
    }
}
