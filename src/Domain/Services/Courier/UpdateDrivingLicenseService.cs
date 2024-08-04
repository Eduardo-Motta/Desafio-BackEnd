using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Courier
{
    public class UpdateDrivingLicenseService : IUpdateDrivingLicenseService
    {
        private readonly ICourierRespository _courierRespository;
        private readonly ILogger _logger;
        private readonly IStorageRepository _storageRepository;

        public UpdateDrivingLicenseService(ICourierRespository courierRespository, IStorageRepository storageRepository, ILogger<CreateCourierService> logger)
        {
            _courierRespository = courierRespository;
            _storageRepository = storageRepository;
            _logger = logger;
        }

        public async Task<Either<Error, string>> UploadFileAsync(string cnpj, Stream stream, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Started Saving image: {fileName}", fileName);

                var courier = await _courierRespository.FindCourierByCnpj(cnpj, cancellationToken);

                if (courier is null)
                {
                    _logger.LogInformation("No courier with this CNPJ not found: {Cnpj}", cnpj);
                    return Either<Error, string>.LeftValue(new Error("Cnpj not found"));
                }
                
                _logger.LogInformation("Uploading driving license image to bucket: {fileName}", fileName);
                var result = await _storageRepository.UploadDrivingLicenseAsync(stream, fileName, cancellationToken);

                _logger.LogInformation("Updating courier entity");
                await _courierRespository.UpdateCourier(courier, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while uploading the driving license image");
                return Either<Error, string>.LeftValue(new Error("An error occurred while uploading the driving license image"));
            }

        }
    }
}
