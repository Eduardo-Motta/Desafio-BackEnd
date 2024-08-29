using Application.Commands;
using Application.Commands.Motorcycle;
using Application.Commands.Motorcycle.Validations;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Application.Handlers.Motorcycle
{
    public class UpdateMotorcycleHandle : IHandler<UpdateMotorcycleCommand>
    {
        private readonly IUpdateMotorcycleService _updateMotorcycleService;
        private readonly ILogger _logger;

        public UpdateMotorcycleHandle(IUpdateMotorcycleService updateMotorcycleService, ILogger<UpdateMotorcycleHandle> logger)
        {
            _updateMotorcycleService = updateMotorcycleService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(UpdateMotorcycleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start of motorcycle update with details: {@Command}", command);

            var validate = await new UpdateMotorcycleValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Motorcycle validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            var result = await _updateMotorcycleService.Update(command.getMotorcycleId(), command.LicensePlate, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponse(result.Right);
        }
    }
}
