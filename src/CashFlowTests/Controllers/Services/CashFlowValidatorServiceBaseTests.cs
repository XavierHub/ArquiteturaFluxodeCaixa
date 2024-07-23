using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation.Results;
using Moq;

namespace EmpXpo.Accounting.Tests
{
    public class CashFlowValidatorServiceBaseTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly ValidatorServiceBase<CashFlow> _validatorService;

        public CashFlowValidatorServiceBaseTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _validatorService = new CashFlowValidatorService(_notifierServiceMock.Object);
        }

        [Fact]
        public async Task IsValidAsync_ModelIsNull_ReturnsFalse()
        {
            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, null);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_ValidModel_ReturnsTrue()
        {
            // Arrange
            var cashFlow = new CashFlow
            {
                SellerId = 1,
                Type = CashFlowType.Credit,
                Amount = 100.0,
                Description = "Valid Description",
                CreatedOn = DateTime.Now
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, cashFlow);

            // Assert
            Assert.True(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_InvalidModel_ReturnsFalse()
        {
            // Arrange
            var cashFlow = new CashFlow
            {
                SellerId = 0, // Invalid SellerId
                Type = (CashFlowType)99, // Invalid Type
                Amount = -10.0, // Invalid Amount
                Description = "", // Invalid Description
                CreatedOn = DateTime.Now
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, cashFlow);

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidValue_ValidValue_ReturnsTrue()
        {
            // Act
            var result = await _validatorService.IsValidValue("TestValue", "Valid Value", x => !string.IsNullOrEmpty(x));

            // Assert
            Assert.True(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidValue_NullValue_ReturnsFalse()
        {
            // Act
            var result = await _validatorService.IsValidValue<string>("TestValue", null, x => !string.IsNullOrEmpty(x));

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidValue_InvalidValue_ReturnsFalse()
        {
            // Act
            var result = await _validatorService.IsValidValue("TestValue", "", x => !string.IsNullOrEmpty(x));

            // Assert
            Assert.False(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }
    }
}
