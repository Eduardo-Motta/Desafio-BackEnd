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
    public class SimulateRentOutServiceTest
    {
        private readonly Mock<ILogger<SimulateRentOutService>> _loggerMock;
        private readonly Mock<ICourierRespository> _courierRepositoryMock;
        private readonly Mock<IPlanRepository> _planRepositoryMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;

        private readonly PlanEntityMock _planEntityMock;

        private readonly string _cnpj;
        private readonly CourierEntity _courier;

        public SimulateRentOutServiceTest()
        {
            _loggerMock = new Mock<ILogger<SimulateRentOutService>>();
            _courierRepositoryMock = new Mock<ICourierRespository>();
            _planRepositoryMock = new Mock<IPlanRepository>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

            _planEntityMock = new PlanEntityMock();

            _cnpj = "54698263000145";
            _courier = new CourierEntity("Carol Price", _cnpj, DateTime.Now.Date.AddYears(-54), "", EDrivingLicenseCategory.AB);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenPlanNotFound()
        {
            ISimulateRentOutService service = new SimulateRentOutService(_planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);

            DateTime expectedReturnDate = DateTime.Now.AddDays(7);
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(_cnpj, cancellationToken))
            .ReturnsAsync(_courier);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync((PlanEntity)null!);

            var result = await service.SimulateRent(_cnpj, Guid.NewGuid(), planSevenDays.Id, expectedReturnDate, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("Plan not found", result.Left.Message);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenCourierNotFound()
        {
            ISimulateRentOutService service = new SimulateRentOutService(_planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "86050852000109";
            DateTime expectedReturnDate = DateTime.Now.AddDays(7);
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync((CourierEntity)null!);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(planSevenDays);

            var result = await service.SimulateRent(cnpj, Guid.NewGuid(), planSevenDays.Id, expectedReturnDate, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("Courier not found", result.Left.Message);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenCourierDoesNotHaveCategoryA()
        {
            ISimulateRentOutService service = new SimulateRentOutService(_planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "07587588000170";
            var courier = new CourierEntity("Carol Price", cnpj, DateTime.Now.Date.AddYears(-54), "", EDrivingLicenseCategory.B);
            DateTime expectedReturnDate = DateTime.Now.AddDays(7);
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync(courier);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(planSevenDays);

            var result = await service.SimulateRent(cnpj, Guid.NewGuid(), planSevenDays.Id, expectedReturnDate, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("Only courier with category A can perform this operation", result.Left.Message);
        }

        [Fact]
        public async Task ShouldNotSimulateRentOutAndReturnExceptionHandling()
        {
            ISimulateRentOutService service = new SimulateRentOutService(_planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "86050852000109";
            DateTime expectedReturnDate = DateTime.Now.AddDays(7);
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ThrowsAsync(new Exception("Repository exception"));

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(planSevenDays);

            var result = await service.SimulateRent(cnpj, Guid.NewGuid(), planSevenDays.Id, expectedReturnDate, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("An error occurred while simulating the rental", result.Left.Message);
        }

        [Fact]
        public async Task ShouldSimulateRentOutWithSevenDaysPlan()
        {
            ISimulateRentOutService service = new SimulateRentOutService(_planRepositoryMock.Object, _courierRepositoryMock.Object, _motorcycleRepositoryMock.Object, _loggerMock.Object);
            string cnpj = "54698263000145";
            DateTime expectedReturnDate = DateTime.Now.AddDays(7);
            var planSevenDays = _planEntityMock.PlanSevenDays;
            var cancellationToken = CancellationToken.None;
            var motorcycle = new MotorcycleEntity(2020, "CG Titan", "AAA-2245");

            _courierRepositoryMock
            .Setup(repository => repository.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync(_courier);

            _planRepositoryMock
            .Setup(repository => repository.FindPlanById(planSevenDays.Id, cancellationToken))
            .ReturnsAsync(planSevenDays);

            _motorcycleRepositoryMock
            .Setup(repository => repository.FindMotorcycleById(motorcycle.Id, cancellationToken))
            .ReturnsAsync(motorcycle);

            var result = await service.SimulateRent(cnpj, motorcycle.Id, planSevenDays.Id, expectedReturnDate, cancellationToken);

            Assert.True(result.IsRight);
            Assert.Equal(210, result.Right.RentalClosure.CostForUsedDays);
            Assert.Equal(0, result.Right.RentalClosure.PenaltyAmountForUnusedDay);
            Assert.Equal(210, result.Right.RentalClosure.TotalPayment);
        }
    }
}
