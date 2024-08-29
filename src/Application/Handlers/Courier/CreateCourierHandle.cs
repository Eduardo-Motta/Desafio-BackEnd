using Application.Commands;
using Application.Commands.Courier;
using Application.Commands.Courier.Validations;
using Domain.Entities;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Application.Handlers.Courier
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
                _logger.LogInformation("Courier validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            _logger.LogInformation("Mapping Courier entity");
            var courier = new CourierEntity(command.Name, command.Cnpj, command.BirthDate, command.DrivingLicense, (Domain.Enums.EDrivingLicenseCategory)command.DrivingLicenseCategory);

            _logger.LogInformation("Mapping User entity");
            var user = new UserEntity(courier.Id, courier.Cnpj, command.Password, Domain.Enums.EUserRole.Courier);

            var result = await _createCourierService.CreateCourier(courier, user, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<Guid>(result.Right);
        }
    }
}
