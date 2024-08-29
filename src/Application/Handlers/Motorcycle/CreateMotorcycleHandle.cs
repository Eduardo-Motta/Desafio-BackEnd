using Application.Commands;
using Application.Commands.Motorcycle;
using Application.Commands.Motorcycle.Validations;
using Domain.Entities;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Application.Handlers.Motorcycle
{
    public class CreateMotorcycleHandle : IHandler<CreateMotorcycleCommand>
    {
        private readonly ICreateMotorcycleService _createMotorcycleService;
        private readonly ILogger _logger;

        public CreateMotorcycleHandle(ICreateMotorcycleService createMotorcycleService, ILogger<CreateMotorcycleHandle> logger)
        {
            _createMotorcycleService = createMotorcycleService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(CreateMotorcycleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start of motorcycle creation with details: {@Command}", command);

            var validate = await new CreateMotorcycleValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Motorcycle validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            _logger.LogInformation("Mapping motorcycle entity");
            var motorcycle = new MotorcycleEntity(command.Year, command.Model, command.LicensePlate);

            var result = await _createMotorcycleService.Create(motorcycle, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<Guid>(result.Right);
        }
    }
}
