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
    public class SellerValidatorServiceBase : ValidatorServiceBase<Seller>
    {
        public SellerValidatorServiceBase(INotifierService notifierService) : base(notifierService)
        {
            // Configurar regras de validação aqui
            _validator.RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(80);
            _validator.RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(50).EmailAddress();
        }
    }

    public class SellerValidatorServiceBaseTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly SellerValidatorServiceBase _validatorService;

        public SellerValidatorServiceBaseTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _validatorService = new SellerValidatorServiceBase(_notifierServiceMock.Object);
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
            var seller = new Seller
            {
                Name = "Valid Name",
                Email = "valid@example.com"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

            // Assert
            Assert.True(result);
            _notifierServiceMock.Verify(n => n.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task IsValidAsync_InvalidModel_ReturnsFalse()
        {
            // Arrange
            var seller = new Seller
            {
                Name = "",
                Email = "invalid-email"
            };

            // Act
            var result = await _validatorService.IsValidAsync(ValidatorType.Create, seller);

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
