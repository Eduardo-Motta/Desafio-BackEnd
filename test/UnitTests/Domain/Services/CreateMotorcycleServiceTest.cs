using Domain.Entities;
using Domain.Events;
using Domain.Messaging;
using Domain.Repositories;
using Domain.Services.Motorcycle;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests.Domain.Services
{
    public class CreateMotorcycleServiceTest
    {
        private readonly Mock<ILogger<CreateMotorcycleService>> _loggerMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly Mock<IMessageBus> _messageBusMock;

        public CreateMotorcycleServiceTest()
        {
            _loggerMock = new Mock<ILogger<CreateMotorcycleService>>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _messageBusMock = new Mock<IMessageBus>();
        }

        [Fact]
        public async Task ShouldCreateNewMotorcycle()
        {
            MotorcycleEntity entity = new MotorcycleEntity(2023, "Yamaha Fazer 250", "ABC1D23");
            var @event = new MotorcycleRegisteredEvent(entity.Id, entity.Year, entity.Model, entity.LicensePlate);

            var cancellationToken = CancellationToken.None;

            var service = new CreateMotorcycleService(_motorcycleRepositoryMock.Object, _messageBusMock.Object, _loggerMock.Object);

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleByLicensePlate("ABC1D23", cancellationToken))
                .ReturnsAsync((MotorcycleEntity)null!);

            _motorcycleRepositoryMock.Setup(repository => repository.CreateMotorcycle(entity, cancellationToken))
           .Returns(Task.CompletedTask);

            _messageBusMock.Setup(bus => bus.PublishEventAsync(@event, cancellationToken))
           .Returns(Task.CompletedTask);

            var result = await service.Create(entity, cancellationToken);

            Assert.True(result.IsRight);
            Assert.IsType<Guid>(result.Right);
        }

        [Fact]
        public async Task ShouldNotCreateMotorcycleWhenLicensePlateAlreadyExists()
        {
            MotorcycleEntity entity = new MotorcycleEntity(2023, "Honda CG 160", "ABC1D23");
            MotorcycleEntity existingMotorcycle = new MotorcycleEntity(2023, "Yamaha Fazer 150", "ABC1D23");

            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleByLicensePlate("ABC1D23", cancellationToken))
                .ReturnsAsync(existingMotorcycle);

            var service = new CreateMotorcycleService(_motorcycleRepositoryMock.Object, _messageBusMock.Object, _loggerMock.Object);

            var result = await service.Create(entity, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("License plate is already in use", result.Left.Message);
        }

        [Fact]
        public async Task ShouldNotCreateMotorcycleAndReturnExceptionHandling()
        {
            MotorcycleEntity entity = new MotorcycleEntity(2024, "Honda CG 160", "PQR5H6");

            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleByLicensePlate(It.IsAny<string>(), cancellationToken))
                .ThrowsAsync(new Exception());

            var service = new CreateMotorcycleService(_motorcycleRepositoryMock.Object, _messageBusMock.Object, _loggerMock.Object);

            var result = await service.Create(entity, cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("An error occurred while creating the motorcycle", result.Left.Message);
        }
    }
}
