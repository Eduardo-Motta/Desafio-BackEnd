using Domain.Commands;
using Domain.Commands.Courier;
using Domain.Commands.Courier.Validations;
using Domain.Entities;
using Domain.Services.Courier;
using Shared.Commands;
using Shared.Handlers;

namespace Domain.Handlers.Courier
{
    public sealed class CreateCourierHandle : IHandler<CreateCourierCommand>
    {
        private readonly CreateCourierService _createCourierService;

        CreateCourierHandle(CreateCourierService createCourierService)
        {
            _createCourierService = createCourierService;
        }

        public async Task<ICommandResult> Handle(CreateCourierCommand command, CancellationToken cancellationToken)
        {
            var validate = await new CreateCourierValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                return new CommandResponseErrors(validate.Errors);
            }

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
