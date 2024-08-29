using Application.Commands;
using Application.Commands.RentOut;
using Application.Commands.RentOut.Validations;
using Domain.Dtos;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Application.Handlers.RentOut
{
    public class CreateRentOutHandle : IHandler<CreateRentOutCommand>
    {
        private readonly ICreateRentOutService _createRentOutService;
        private readonly ILogger _logger;

        public CreateRentOutHandle(ICreateRentOutService createRentOutService, ILogger<CreateRentOutHandle> logger)
        {
            _createRentOutService = createRentOutService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(CreateRentOutCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start of rent out creation to courier with CNPJ: {Cnpj}", command.Cnpj);

            var validate = await new CreateRentOutValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Rent out validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            var result = await _createRentOutService.Create(command.Cnpj, command.MotorcycleId, command.PlanId, command.ExpectedEndDate, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            var dto = new RentOutDto(result.Right.Id
                , result.Right.PlanId
                , result.Right.MotorcycleId
                , result.Right.StartDate
                , result.Right.ExpectedEndDate
                , result.Right.TotalPayment);

            return new CommandResponseData<RentOutDto>(dto);
        }
    }
}
