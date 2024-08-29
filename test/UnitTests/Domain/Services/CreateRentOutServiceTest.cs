using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services.Contracts;
using Domain.Services.RentOut;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Utils;
using UnitTests.Domain.Mocks;

namespace UnitTests.Domain.Services
{
    public class CreateRentOutServiceTest
    {
        private readonly Mock<ILogger<CreateRentOutService>> _loggerMock;
        private readonly Mock<ICourierRespository> _courierRepositoryMock;
        private readonly Mock<IPlanRepository> _planRepositoryMock;
        private readonly Mock<IRentReposotory> _rentOutReposotoryMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;

        private readonly PlanEntityMock _planEntityMock;

        private readonly string _cnpj;
        private readonly CourierEntity _courier;

        public CreateRentOutServiceTest()
        {
            _loggerMock = new Mock<ILogger<CreateRentOutService>>();
            _courierRepositoryMock = new Mock<ICourierRespository>();
            _planRepositoryMock = new Mock<IPlanRepository>();
            _rentOutReposotoryMock = new Mock<IRentReposotory>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

            _planEntityMock = new PlanEntityMock();

            _cnpj = "54698263000145";
            _courier = new CourierEntity("Carol Price", _cnpj, DateTime.Now.Date.AddYears(-54), "", EDrivingLicenseCategory.A);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenPlanNotFound()
        {
            ICreateRentOutService service = new CreateRentOutService(_rentOutReposotoryMock.Object, _planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);

            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repository => repository.FindCourierByCnpj(_cnpj, cancellationToken))
            .ReturnsAsync(_courier);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync((PlanEntity)null!);

            _rentOutReposotoryMock
           .Setup(repository => repository.CreateRent(It.IsAny<RentEntity>(), cancellationToken))
           .Returns(Task.CompletedTask);

            var result = await service.Create(_cnpj, Guid.NewGuid(), planSevenDays.Id, DateTime.Now.AddDays(7).Date, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("Plan not found", result.Left.Message);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenCourierNotFound()
        {
            ICreateRentOutService service = new CreateRentOutService(_rentOutReposotoryMock.Object, _planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "86050852000109";
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync((CourierEntity)null!);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(planSevenDays);

            _rentOutReposotoryMock
           .Setup(repository => repository.CreateRent(It.IsAny<RentEntity>(), cancellationToken))
           .Returns(Task.CompletedTask);

            var result = await service.Create(cnpj, Guid.NewGuid(), planSevenDays.Id, DateTime.Now.AddDays(7).Date, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("Courier not found", result.Left.Message);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenCourierDoesNotHaveCategoryA()
        {
            ICreateRentOutService service = new CreateRentOutService(_rentOutReposotoryMock.Object, _planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "07587588000170";
            var courier = new CourierEntity("Carol Price", cnpj, DateTime.Now.Date.AddYears(-54), "", EDrivingLicenseCategory.B);
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync(courier);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(planSevenDays);

            _rentOutReposotoryMock
           .Setup(repository => repository.CreateRent(It.IsAny<RentEntity>(), cancellationToken))
           .Returns(Task.CompletedTask);

            var result = await service.Create(cnpj, Guid.NewGuid(), planSevenDays.Id, DateTime.Now.AddDays(7).Date, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("Only courier with category A can perform this operation", result.Left.Message);
        }

        [Fact]
        public async Task ShouldNotCreateRentOutAndReturnExceptionHandling()
        {
            ICreateRentOutService service = new CreateRentOutService(_rentOutReposotoryMock.Object, _planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "86050852000109";
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ThrowsAsync(new Exception("Repository exception"));

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(planSevenDays);

            _rentOutReposotoryMock
           .Setup(repository => repository.CreateRent(It.IsAny<RentEntity>(), cancellationToken))
           .Returns(Task.CompletedTask);

            var result = await service.Create(cnpj, Guid.NewGuid(), planSevenDays.Id, DateTime.Now.AddDays(7).Date, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("An error occurred while creating the rental", result.Left.Message);
        }

        [Fact]
        public async Task ShouldCreateteRentOut()
        {
            ICreateRentOutService service = new CreateRentOutService(_rentOutReposotoryMock.Object, _planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "54698263000145";
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;
            var motorcycle = new MotorcycleEntity(2020, "CG Titan", "AAA-2245");

            _courierRepositoryMock
            .Setup(repository => repository.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync(_courier);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(_planEntityMock.PlanSevenDays);

            _rentOutReposotoryMock
           .Setup(repository => repository.CreateRent(It.IsAny<RentEntity>(), cancellationToken))
           .Returns(Task.CompletedTask);

            _motorcycleRepositoryMock
            .Setup(repository => repository.FindMotorcycleById(motorcycle.Id, cancellationToken))
            .ReturnsAsync(motorcycle);

            var result = await service.Create(cnpj, motorcycle.Id, planSevenDays.Id, DateTime.Now.AddDays(7).Date, cancellationToken);

            Assert.True(result.IsRight);
            Assert.NotNull(result.Right.RentalClosure);
            Assert.Equal(DateTime.Now.AddDays(1).Date, result.Right.StartDate);
            Assert.Equal(DateTime.Now.AddDays(7).Date, result.Right.ExpectedEndDate);
            Assert.Equal(210, result.Right.TotalPayment);
        }
    }
}
