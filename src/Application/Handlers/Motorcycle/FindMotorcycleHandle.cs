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
    public class FindMotorcycleHandle : IHandler<FindMotorcycleByLicensePlateCommand>, IHandler<FindAllMotorcyclesCommand>, IHandler<FindAllAvailableMotorcyclesCommand>
    {
        private readonly ILogger _logger;
        private readonly IFindMotorcycleService _findMotorcycleService;

        public FindMotorcycleHandle(IFindMotorcycleService findMotorcycleService, ILogger<FindMotorcycleHandle> logger)
        {
            _findMotorcycleService = findMotorcycleService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(FindAllMotorcyclesCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting search for all motorcycles");

            var result = await _findMotorcycleService.All(cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<IEnumerable<MotorcycleEntity>>(result.Right);
        }

        public async Task<ICommandResult> Handle(FindMotorcycleByLicensePlateCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting motorcycle search by id: {Command}", command);

            _logger.LogInformation("Validating motorcycle command");
            var validate = await new FindMotorcycleByLicensePlateValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Motorcycle validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            var result = await _findMotorcycleService.FindByLicensePlate(command.LicensePlate, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<MotorcycleEntity>(result.Right);
        }

        public async Task<ICommandResult> Handle(FindAllAvailableMotorcyclesCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting search for all available motorcycles");

            var result = await _findMotorcycleService.AllAvailableMotorcycles(cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<IEnumerable<MotorcycleEntity>>(result.Right);
        }
    }
}
