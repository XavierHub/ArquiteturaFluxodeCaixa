using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation.Results;
using Moq;

namespace EmpXpo.Accounting.Tests
{
    public class CashFlowValidatorServiceTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly CashFlowValidatorService _validatorService;

        public CashFlowValidatorServiceTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _validatorService = new CashFlowValidatorService(_notifierServiceMock.Object);
        }

        [Fact]
        public async Task IsValidAsync_ValidCashFlow_ReturnsTrue()
        {
            // Arrange
            var cashFlow = new CashFlow
            {
                SellerId = 1,
                Type = CashFlowType.Credit,
                Amount = 100.0,
                Description = "Valid Description"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, cashFlow);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsValidAsync_InvalidSellerId_ReturnsFalse()
        {
            // Arrange
            var cashFlow = new CashFlow
            {
                SellerId = 0,
                Type = CashFlowType.Credit,
                Amount = 100.0,
                Description = "Valid Description"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, cashFlow);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_InvalidType_ReturnsFalse()
        {
            // Arrange
            var cashFlow = new CashFlow
            {
                SellerId = 1,
                Type = (CashFlowType)99, // Invalid Type
                Amount = 100.0,
                Description = "Valid Description"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, cashFlow);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_InvalidDescription_ReturnsFalse()
        {
            // Arrange
            var cashFlow = new CashFlow
            {
                SellerId = 1,
                Type = CashFlowType.Credit,
                Amount = 100.0,
                Description = "" // Invalid Description
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, cashFlow);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_InvalidAmount_ReturnsFalse()
        {
            // Arrange
            var cashFlow = new CashFlow
            {
                SellerId = 1,
                Type = CashFlowType.Credit,
                Amount = -100.0, // Invalid Amount
                Description = "Valid Description"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, cashFlow);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }
    }
}
