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
    public class SimulateRentOutHandle : IHandler<SimulateRentOutCommand>
    {
        private readonly ISimulateRentOutService _simulateRentOutService;
        private readonly ILogger _logger;

        public SimulateRentOutHandle(ISimulateRentOutService simulateRentOutService, ILogger<SimulateRentOutHandle> logger)
        {
            _simulateRentOutService = simulateRentOutService;
            _logger = logger;
        }
        public async Task<ICommandResult> Handle(SimulateRentOutCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start of rent out simulation to courier with CNPJ: {Cnpj}", command.Cnpj);

            var validate = await new SimulateRentOutValidation().ValidateAsync(command, cancellationToken);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Rent out validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            var result = await _simulateRentOutService.SimulateRent(command.Cnpj, command.MotorcycleId, command.PlanId, command.ExpectedReturnDate, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            var dto = new RentOutSimulatedDto(result.Right.RentalClosure.ExceededDays
                , result.Right.TotalPayment
                , result.Right.RentalClosure.CostForUsedDays
                , result.Right.RentalClosure.PenaltyAmountForUnusedDay
                , result.Right.RentalClosure.TotalAdditionalDailyAmount);

            return new CommandResponseData<RentOutSimulatedDto>(dto);
        }
    }
}
