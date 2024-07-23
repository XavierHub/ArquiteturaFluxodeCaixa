using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation.Results;
using Moq;

namespace EmpXpo.Accounting.Tests
{
    public class ReportValidatorServiceTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly ReportValidatorService _validatorService;

        public ReportValidatorServiceTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _validatorService = new ReportValidatorService(_notifierServiceMock.Object);
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
            var report = new Report
            {
                SellerId = 1,
                Debit = 100.0,
                Credit = 50.0,
                DailyBalance = 50.0,
                Reference = DateTime.Now.AddDays(-1),
                ProcessingDate = DateTime.Now
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, report);

            // Assert
            Assert.True(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_InvalidModel_ReturnsFalse()
        {
            // Arrange
            var report = new Report
            {
                SellerId = 0, // Invalid SellerId                
                Credit = -5.0, // Invalid Credit                
                Reference = DateTime.Now.AddDays(-1),
                ProcessingDate = DateTime.Now
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, report);

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
