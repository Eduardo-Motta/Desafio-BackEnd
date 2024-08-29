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
    public class CompleteRentHandle : IHandler<CompleteRentCommand>
    {
        private readonly ICompleteRentService _completeRentService;
        private readonly ILogger _logger;

        public CompleteRentHandle(ICompleteRentService completeRentService, ILogger<CompleteRentHandle> logger)
        {
            _completeRentService = completeRentService;
            _logger = logger;
        }
        public async Task<ICommandResult> Handle(CompleteRentCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start complete rent out to courier with CNPJ: {Cnpj}", command.Cnpj);

            var validate = await new CompleteRentValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Rent out validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            var result = await _completeRentService.Complete(command.RentId, cancellationToken);

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
