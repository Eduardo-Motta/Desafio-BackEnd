using Application.Commands;
using Application.Commands.Courier;
using Application.Commands.Courier.Validations;
using Domain.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Application.Handlers.Courier
{
    public class UploadDrivingLicenseHandle : IHandler<UploadDrivingLicenseCommand>
    {
        private readonly IUpdateDrivingLicenseService _updateDrivingLicenseService;
        private readonly ILogger _logger;

        public UploadDrivingLicenseHandle(IUpdateDrivingLicenseService updateDrivingLicenseService, ILogger<UploadDrivingLicenseHandle> logger)
        {
            _updateDrivingLicenseService = updateDrivingLicenseService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(UploadDrivingLicenseCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to upload the driving license image for courier with CNPJ: {Cnpj}", command.GetCnpj());
            var validate = await new UploadDrivingLicenseValidation().ValidateAsync(command);

            if (!validate.IsValid)
            {
                _logger.LogInformation("Driving license image validated with errors: {@Errors}", validate.Errors);
                return new CommandResponseErrors(validate.Errors);
            }

            string fileName = GenerateFileName(command.GetCnpj(), command.File);

            _logger.LogInformation("Converting image to stream");
            using Stream stream = ConvertFileToStream(command.File);

            var result = await _updateDrivingLicenseService.UploadFileAsync(command.GetCnpj(), stream, fileName, cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<string>(result.Right);
        }

        private Stream ConvertFileToStream(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            return stream;
        }

        private string GenerateFileName(string cnpj, IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            return $"{cnpj}-{Guid.NewGuid()}{fileExtension}";
        }
    }
}
