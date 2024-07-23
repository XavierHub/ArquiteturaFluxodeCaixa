using EmpXpo.Accounting.Application;
using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation.Results;
using Moq;

namespace CashFlowApiTests.Application
{
    public class CashFlowApplicationTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly Mock<IRepository<CashFlow>> _cashFlowRepositoryMock;
        private readonly Mock<IRepository<Seller>> _sellerRepositoryMock;
        private readonly ICashFlowApplication _cashFlowApplication;
        private readonly IValidatorService<CashFlow> _validatorService;

        public CashFlowApplicationTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _sellerRepositoryMock = new Mock<IRepository<Seller>>();
            _validatorService = new CashFlowValidatorService(_notifierServiceMock.Object);
            _cashFlowRepositoryMock = new Mock<IRepository<CashFlow>>();
            _cashFlowApplication = new CashFlowApplication(_notifierServiceMock.Object, _cashFlowRepositoryMock.Object, _sellerRepositoryMock.Object, _validatorService);

            _cashFlowRepositoryMock.Setup(repo => repo.Insert(It.IsAny<CashFlow>()))
                                   .ReturnsAsync(10);
        }

        [Theory]
        [InlineData(CashFlowType.Debit)]
        [InlineData(CashFlowType.Credit)]
        public async Task Create_ValidCashFlow_ShouldReturnCreatedCashFlow(CashFlowType type)
        {
            _sellerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                                 .Returns(Task.Run(() => new Seller { Id = 1 }));

            // Arrange
            var model = new CashFlow { SellerId = 1, Amount = 100, Description = "Importante", Type = type };

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.True(result.Id > 0);
            Assert.NotEqual(DateTime.MinValue, result.CreatedOn);
        }

        [Theory]
        [InlineData(CashFlowType.Debit)]
        [InlineData(CashFlowType.Credit)]
        public async Task Create_MissingSellerId_ShouldNotCreateCashFlowAndLogNotification(CashFlowType type)
        {
            // Arrange
            _notifierServiceMock.Setup(notifier => notifier.Add(It.IsAny<ValidationResult>())).Verifiable();

            var model = new CashFlow { Amount = 100, Description = "Importante", Type = type };

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(DateTime.MinValue, result.CreatedOn);
            _notifierServiceMock.Verify(notifier => notifier.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidCashFlow_ShouldLogNotification()
        {
            // Arrange
            _notifierServiceMock.Setup(notifier => notifier.Add(It.IsAny<ValidationResult>())).Verifiable();

            var model = new CashFlow();

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(DateTime.MinValue, result.CreatedOn);
            _notifierServiceMock.Verify(notifier => notifier.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task Create_InvalidSellerId_ShouldReturnNotification()
        {
            // Arrange
            _sellerRepositoryMock.Setup(repo => repo.GetById(1))
                                 .Returns(Task.Run(() => new Seller { Id = 1 }));

            _notifierServiceMock.Setup(notifier => notifier.Add(It.IsAny<ValidationResult>())).Verifiable();

            var model = new CashFlow();

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.Equal(0, result.Id);
            _notifierServiceMock.Verify(notifier => notifier.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Theory]
        [InlineData(CashFlowType.Debit)]
        [InlineData(CashFlowType.Credit)]
        public async Task Create_CashFlowWithId_ShouldReturnCreatedCashFlow(CashFlowType type)
        {
            // Arrange
            _sellerRepositoryMock.Setup(repo => repo.GetById(It.IsAny<int>()))
                                 .Returns(Task.Run(() => new Seller { Id = 1 }));

            var model = new CashFlow { SellerId = 1, Id = 10, Amount = 100, Description = "Conta de Luz", Type = type };

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.True(result.Id > 0);
            Assert.NotEqual(DateTime.MinValue, result.CreatedOn);
        }

        [Fact]
        public async Task Create_NullCashFlowModel_ShouldNotCreateCashFlowAndLogNotification()
        {
            // Arrange
            _notifierServiceMock.Setup(notifier => notifier.Add(It.IsAny<ValidationResult>())).Verifiable();

            CashFlow? model = null;

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(DateTime.MinValue, result.CreatedOn);
            _notifierServiceMock.Verify(notifier => notifier.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task Create_CashFlowWithZeroAmount_ShouldNotCreateCashFlowAndLogNotification()
        {
            // Arrange
            _notifierServiceMock.Setup(notifier => notifier.Add(It.IsAny<ValidationResult>())).Verifiable();

            var model = new CashFlow { SellerId = 1, Amount = 0, Description = "Muito Importante", Type = CashFlowType.Debit };

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(DateTime.MinValue, result.CreatedOn);
            _notifierServiceMock.Verify(notifier => notifier.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task Create_CashFlowWithExcessiveDescription_ShouldNotCreateCashFlowAndLogNotification()
        {
            // Arrange
            _notifierServiceMock.Setup(notifier => notifier.Add(It.IsAny<ValidationResult>())).Verifiable();
            _cashFlowRepositoryMock.Setup(repo => repo.Insert(It.IsAny<CashFlow>()))
                                   .Throws<Exception>();

            var model = new CashFlow { SellerId = 1, Amount = 200, Description = new string('A', 500), Type = CashFlowType.Debit };

            // Act
            var result = await _cashFlowApplication.Create(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CashFlow>(result);
            Assert.Equal(0, result.Id);
            Assert.Equal(DateTime.MinValue, result.CreatedOn);
            _notifierServiceMock.Verify(notifier => notifier.Add(It.IsAny<ValidationResult>()), Times.Once);
        }
    }
}
