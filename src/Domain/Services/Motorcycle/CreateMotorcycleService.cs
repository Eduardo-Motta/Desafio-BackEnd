using Domain.Entities;
using Domain.Events;
using Domain.Messaging;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Motorcycle
{
    public class CreateMotorcycleService : ICreateMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMessageBus _messageBus;
        private readonly ILogger _logger;

        public CreateMotorcycleService(IMotorcycleRepository motorcycleRepository, IMessageBus messageBus, ILogger<CreateMotorcycleService> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _messageBus = messageBus;
            _logger = logger;
        }

        public async Task<Either<Error, Guid>> Create(MotorcycleEntity entity, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating motorcycle with license plate: {LicensePlate}", entity.LicensePlate);

                var motorcycleByLicensePlate = await _motorcycleRepository.FindMotorcycleByLicensePlate(entity.LicensePlate, cancellationToken);

                if (motorcycleByLicensePlate is not null)
                {
                    _logger.LogWarning("A motorcycle with this license plate already exists: {LicensePlate}", entity.LicensePlate);
                    return Either<Error, Guid>.LeftValue(new Error("License plate is already in use"));
                }
                
                await _motorcycleRepository.CreateMotorcycle(entity, cancellationToken);

                _logger.LogInformation("Motorcycle created sucessfullly with license plate: {LicensePlate}", entity.LicensePlate);

                var @event = new MotorcycleRegisteredEvent(entity.Id, entity.Year, entity.Model, entity.LicensePlate);

                await _messageBus.PublishEventAsync(@event, cancellationToken);
                _logger.LogInformation("Event published sucessfullly");

                return Either<Error, Guid>.RightValue(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while creating the motorcycle");
                return Either<Error, Guid>.LeftValue(new Error("An error occurred while creating the motorcycle"));
            }
        }
    }
}
