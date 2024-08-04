using Shared.Utils;

namespace Domain.Repositories
{
    public interface IStorageRepository
    {
        Task<Either<Error, string>> UploadDrivingLicenseAsync(Stream stream, string fileName, CancellationToken cancellationToken);
    }
}
