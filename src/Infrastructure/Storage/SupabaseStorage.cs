using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Storage
{
    public class SupabaseStorage : IStorageRepository
    {
        private readonly ILogger _logger;
        private readonly Supabase.Client _supabaseClient;
        private readonly string _bucketName;
        private readonly string _supabaseUrl;

        public SupabaseStorage(string supabaseUrl, string supabaseKey, string bucketName, ILogger<SupabaseStorage> logger)
        {
            _logger = logger;
            _bucketName = bucketName;
            _supabaseUrl = supabaseUrl;

            _supabaseClient = new Supabase.Client(supabaseUrl, supabaseKey);
        }
        public async Task<string> UploadDrivingLicenseAsync(Stream stream, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                byte[] fileBytes;
                var storage = _supabaseClient.Storage;
                var bucket = storage.From(_bucketName);

                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                var filePath = Path.Combine("driving-license", fileName);

                var response = await bucket.Upload(fileBytes, filePath);

                if (response == null)
                {
                    throw new InvalidOperationException("Error uploading file to Supabase Storage");
                }

                return $"{_supabaseUrl.TrimEnd()}/storage/v1/object/public/{response.TrimStart()}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to Supabase Storage");
                throw;
            }
        }
    }
}
