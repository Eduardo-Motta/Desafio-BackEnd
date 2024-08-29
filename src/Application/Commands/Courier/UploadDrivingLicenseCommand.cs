using Microsoft.AspNetCore.Http;
using Shared.Commands;

namespace Application.Commands.Courier
{
    public class UploadDrivingLicenseCommand : ICommand
    {
        private string _cnpj = string.Empty;

        public IFormFile File { get; set; } = default!;

        public string GetCnpj()
        {
            return _cnpj;
        }

        public void SetCnpj(string cnpj)
        {
            _cnpj = cnpj;
        }
    }
}
