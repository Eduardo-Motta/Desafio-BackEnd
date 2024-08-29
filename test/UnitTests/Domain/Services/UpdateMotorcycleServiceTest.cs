using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Motorcycle;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Utils;

namespace UnitTests.Domain.Services
{
    public class UpdateMotorcycleServiceTest
    {
        private readonly Mock<ILogger<UpdateMotorcycleService>> _loggerMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;

        public UpdateMotorcycleServiceTest()
        {
            _loggerMock = new Mock<ILogger<UpdateMotorcycleService>>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        }

        [Fact]
        public async Task ShouldUpdateExistingMotorcycle()
        {
            MotorcycleEntity entity = new MotorcycleEntity(2023, "Honda Pop 100", "CGH2J34");
            var cancellationToken = CancellationToken.None;
            var newLicensePlate = "FAZ7B89";

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleById(entity.Id, cancellationToken))
                .ReturnsAsync(entity);

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleByLicensePlate(newLicensePlate, cancellationToken))
                .ReturnsAsync((MotorcycleEntity)null!);

            _motorcycleRepositoryMock.Setup(repository => repository.CreateMotorcycle(entity, cancellationToken))
           .Returns(Task.CompletedTask);

            var service = new UpdateMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.Update(entity.Id, newLicensePlate, cancellationToken);

            Assert.True(result.IsRight);
            Assert.IsType<bool>(result.Right);
            Assert.True(result.Right);
            Assert.Equal(newLicensePlate, entity.LicensePlate);
        }

        [Fact]
        public async Task ShouldNotUpdateWhenMotorcycleNotExists()
        {
            var motorcycleId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;
            var newLicensePlate = "CGK5M67";

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleById(motorcycleId, cancellationToken))
                .ReturnsAsync((MotorcycleEntity)null!);

            var service = new UpdateMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.Update(motorcycleId, newLicensePlate, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("Not found", result.Left.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateWhenLicensePlateAlreadyExists()
        {
            MotorcycleEntity entity = new MotorcycleEntity(2023, "Honda Biz 125", "CGH2J34");
            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleById(entity.Id, cancellationToken))
                .ReturnsAsync(entity);

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleByLicensePlate("FAZ7B89", cancellationToken))
                .ReturnsAsync(new MotorcycleEntity(2024, "Yamaha Fazer 150", "FAZ7B89"));

            _motorcycleRepositoryMock.Setup(repository => repository.CreateMotorcycle(entity, cancellationToken))
           .Returns(Task.CompletedTask);

            var service = new UpdateMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.Update(entity.Id, "FAZ7B89", cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("License plate is already in use", result.Left.Message);
        }

        [Fact]
        public async Task ShouldNotUpdateAndReturnExceptionHandling()
        {
            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleById(It.IsAny<Guid>(), cancellationToken))
                .ThrowsAsync(new Exception());

            var service = new UpdateMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.Update(Guid.NewGuid(), "CGK5M67", cancellationToken);

            Assert.True(result.IsLeft);
            Assert.IsType<Error>(result.Left);
            Assert.Equal("An error occurred while updating the motorcycle", result.Left.Message);
        }
    }
}
