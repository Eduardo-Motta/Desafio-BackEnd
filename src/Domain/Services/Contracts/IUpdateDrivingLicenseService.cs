using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IUpdateDrivingLicenseService
    {
        Task<Either<Error, string>> UploadFileAsync(string cnpj, Stream stream, string fileName, CancellationToken cancellationToken);
    }
}
