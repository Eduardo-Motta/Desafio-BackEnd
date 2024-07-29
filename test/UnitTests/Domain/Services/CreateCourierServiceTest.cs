using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services.Courier;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Utils;

namespace UnitTests.Domain.Services
{
    public class CreateCourierServiceTest
    {
        private readonly Mock<ILogger<CreateCourierService>> _loggerMock;
        private readonly Mock<ICourierRespository> _courierRepositoryMock;

        public CreateCourierServiceTest()
        {
            _loggerMock = new Mock<ILogger<CreateCourierService>>();
            _courierRepositoryMock = new Mock<ICourierRespository>();
        }

        [Fact]
        public async void ShouldCreateCourier()
        {
            var cnpj = "21994499000170";
            var drivingLicense = "46687774359";
            var courierToCreate = new CourierEntity("Gemma Gummer", cnpj, DateTime.Now.Date.AddYears(-25), drivingLicense, EDrivingLicenseCategory.A);
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync((CourierEntity)null!);

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByDrivingLicense(drivingLicense, cancellationToken))
            .ReturnsAsync((CourierEntity)null!);

            _courierRepositoryMock
           .Setup(repo => repo.CreateCourier(courierToCreate, cancellationToken))
           .Returns(Task.CompletedTask);

            var service = new CreateCourierService(_courierRepositoryMock.Object, _loggerMock.Object);

            var result = await service.CreateCourier(courierToCreate, cancellationToken);

            Assert.True(result.IsRight);
            Assert.IsType<Guid>(result.Right);
            Assert.Equal(courierToCreate.Id, result.Right);
        }

        [Fact]
        public async void ShouldNotCreateCourierWithExistingCnpj()
        {
            var cnpj = "13523481000161";
            var drivingLicense = "51269486572";
            var expectedCourierByCnpj = new CourierEntity("Arlyss Conner", cnpj, DateTime.Now.Date.AddYears(-41), drivingLicense, EDrivingLicenseCategory.A);
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync(expectedCourierByCnpj);

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByDrivingLicense(drivingLicense, cancellationToken))
            .ReturnsAsync((CourierEntity)null!);


            var service = new CreateCourierService(_courierRepositoryMock.Object, _loggerMock.Object);

            var result = await service.CreateCourier(expectedCourierByCnpj, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("Cnpj is already in use", result.Left.Message);
        }

        [Fact]
        public async void ShouldNotCreateCourierWithExistingDrivingLicense()
        {
            var cnpj = "95300266000166";
            var drivingLicense = "34380775440";
            var expectedCourierByDrivingLicense = new CourierEntity("Odon Curvin", cnpj, DateTime.Now.Date.AddYears(-23), drivingLicense, EDrivingLicenseCategory.A);
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ReturnsAsync((CourierEntity)null!);

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByDrivingLicense(drivingLicense, cancellationToken))
            .ReturnsAsync(expectedCourierByDrivingLicense);

            var service = new CreateCourierService(_courierRepositoryMock.Object, _loggerMock.Object);

            var result = await service.CreateCourier(expectedCourierByDrivingLicense, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("Driving License is already in use", result.Left.Message);
        }

        [Fact]
        public async void ShouldNotCreateCourierAndReturnExceptionHandling()
        {
            var cnpj = "95844878000110";
            var drivingLicense = "98003571272";
            var expectedCourierByDrivingLicense = new CourierEntity("Fanny Atkins", cnpj, DateTime.Now.Date.AddYears(-50), drivingLicense, EDrivingLicenseCategory.A);
            var cancellationToken = CancellationToken.None;

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
            .ThrowsAsync(new Exception("Repository exception"));

            _courierRepositoryMock
            .Setup(repo => repo.FindCourierByDrivingLicense(drivingLicense, cancellationToken))
            .ReturnsAsync((CourierEntity)null!);

            var service = new CreateCourierService(_courierRepositoryMock.Object, _loggerMock.Object);

            var result = await service.CreateCourier(expectedCourierByDrivingLicense, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("An error occurred while creating the courier", result.Left.Message);
        }
    }
}
