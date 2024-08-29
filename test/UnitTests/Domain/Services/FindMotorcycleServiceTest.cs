using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Motorcycle;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Utils;
using System.Collections.Generic;

namespace UnitTests.Domain.Services
{
    public class FindMotorcycleServiceTest
    {
        private readonly Mock<ILogger<FindMotorcycleService>> _loggerMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;

        public FindMotorcycleServiceTest()
        {
            _loggerMock = new Mock<ILogger<FindMotorcycleService>>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        }

        [Fact]
        public async Task ShouldReturAListOfMotorcycles()
        {
            var entities = new List<MotorcycleEntity>()
            {
                new MotorcycleEntity(2023, "Yamaha Fazer 250", "CGH2J34"),
                new MotorcycleEntity(2024, "Honda CG 160", "FAZ7B89"),
                new MotorcycleEntity(2020, "Yamaha Fazer 150", "CGP9X12")
            };

            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.AllMotorcycles(cancellationToken))
                .ReturnsAsync(entities);

            var service = new FindMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.All(cancellationToken);

            Assert.True(result.IsRight);
            Assert.NotEmpty(result.Right);
        }

        [Fact]
        public async Task ShouldReturAMotorcycleByLicensePlate()
        {
            MotorcycleEntity entity = new MotorcycleEntity(2023, "Yamaha Fazer 250", "CGH2J34");
            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleByLicensePlate("CGH2J34", cancellationToken))
                .ReturnsAsync(entity);

            var service = new FindMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.FindByLicensePlate("CGH2J34", cancellationToken);

            Assert.True(result.IsRight);
            Assert.IsType<MotorcycleEntity>(result.Right);
            Assert.Equal(2023, entity.Year);
            Assert.Equal("Yamaha Fazer 250", entity.Model);
            Assert.Equal("CGH2J34", entity.LicensePlate);
        }

        [Fact]
        public async Task ShouldReturnExceptionHandlingWhenFindMotorcycleByLicensePlate()
        {
            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.FindMotorcycleByLicensePlate("CGH2J34", cancellationToken))
                .ThrowsAsync(new Exception());

            var service = new FindMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.FindByLicensePlate("CGH2J34", cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("An error occurred while searching for the motorcycle", result.Left.Message);
        }

        [Fact]
        public async Task ShouldReturnExceptionHandlingWhenSearchAllMotorcycles()
        {
            var cancellationToken = CancellationToken.None;

            _motorcycleRepositoryMock.Setup(repository => repository.AllMotorcycles(cancellationToken))
                .ThrowsAsync(new Exception());

            var service = new FindMotorcycleService(_motorcycleRepositoryMock.Object, _loggerMock.Object);

            var result = await service.All(cancellationToken);

            Assert.True(result.IsLeft);
            Assert.Equal("An error occurred while searching for the motorcycles", result.Left.Message);
        }
    }
}
