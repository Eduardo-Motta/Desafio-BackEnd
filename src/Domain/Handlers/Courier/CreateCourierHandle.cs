using Domain.Commands;
using Domain.Commands.Courier;
using Domain.Commands.Courier.Validations;
using Domain.Entities;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Domain.Handlers.Courier
{
    public sealed class CreateCourierHandle : IHandler<CreateCourierCommand>
    {
        private readonly ICreateCourierService _createCourierService;
        private readonly ILogger _logger;

        public CreateCourierHandle(ICreateCourierService createCourierService, ILogger<CreateCourierHandle> logger)
        {
            _createCourierService = createCourierService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(CreateCourierCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start of courier creation with CNPJ: {Cnpj}", command.Cnpj);

            var validate = await new CreateCourierValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Courier validated with errors: {Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            _logger.LogInformation("Mapping Courier entity");
            var courier = new CourierEntity(command.Name, command.Cnpj, command.BirthDate, command.DrivingLicense, command.DrivingLicenseCategory);

            var result = await _createCourierService.CreateCourier(courier, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<Guid>(result.Right);
        }
    }
}
