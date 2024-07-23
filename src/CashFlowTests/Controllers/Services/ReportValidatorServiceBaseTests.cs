using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace EmpXpo.Accounting.Tests
{
    // Classe derivada para facilitar a configuração das regras de validação
    public class ReportValidatorServiceBase : ValidatorServiceBase<Report>
    {
        public ReportValidatorServiceBase(INotifierService notifierService) : base(notifierService)
        {
            // Configurar regras de validação aqui
            _validator.RuleFor(x => x.SellerId).GreaterThan(0);
            _validator.RuleFor(x => x.Debit).GreaterThanOrEqualTo(0);
            _validator.RuleFor(x => x.Credit).GreaterThanOrEqualTo(0);
            _validator.RuleFor(x => x.DailyBalance).GreaterThanOrEqualTo(0);
            _validator.RuleFor(x => x.Reference).LessThanOrEqualTo(DateTime.Now);
            _validator.RuleFor(x => x.ProcessingDate).LessThanOrEqualTo(DateTime.Now);
        }
    }

    public class ReportValidatorServiceBaseTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly ReportValidatorServiceBase _validatorService;

        public ReportValidatorServiceBaseTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _validatorService = new ReportValidatorServiceBase(_notifierServiceMock.Object);
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
                ProcessingDate = DateTime.Now.AddMinutes(-10)
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
                Debit = -10.0, // Invalid Debit
                Credit = -5.0, // Invalid Credit
                DailyBalance = -15.0, // Invalid DailyBalance
                Reference = DateTime.Now.AddDays(1), // Future date to make it invalid
                ProcessingDate = DateTime.Now.AddDays(1) // Future date to make it invalid
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
