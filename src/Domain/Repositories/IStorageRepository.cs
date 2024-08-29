namespace Domain.Repositories
{
    public interface IStorageRepository
    {
        Task<string> UploadDrivingLicenseAsync(Stream stream, string fileName, CancellationToken cancellationToken);
    }
}
