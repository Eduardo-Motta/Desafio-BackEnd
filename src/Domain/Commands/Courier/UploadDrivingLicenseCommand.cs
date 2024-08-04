using Microsoft.AspNetCore.Http;
using Shared.Commands;

namespace Domain.Commands.Courier
{
    public class UploadDrivingLicenseCommand : ICommand
    {
        public IFormFile File { get; set; } = default!;
        public string Cnpj { get; private set; } = string.Empty;

        public void SetCourierCnpj(string cnpj)
        {
            Cnpj = cnpj;
        }
    }
}
