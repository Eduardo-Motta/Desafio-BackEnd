using Application.Commands;
using Application.Commands.Motorcycle;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Application.Handlers.Motorcycle
{
    public class DeleteMotorcycleHandle : IHandler<DeleteMotorcycleCommand>
    {
        private readonly IDeleteMotorcycleService _deleteMotorcycleService;
        private readonly ILogger _logger;

        public DeleteMotorcycleHandle(IDeleteMotorcycleService deleteMotorcycleService, ILogger<DeleteMotorcycleHandle> logger)
        {
            _deleteMotorcycleService = deleteMotorcycleService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(DeleteMotorcycleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start deletion of motorcycle with details: {@Command}", command);

            var result = await _deleteMotorcycleService.Delete(command.Id, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponse(result.Right);
        }
    }
}
