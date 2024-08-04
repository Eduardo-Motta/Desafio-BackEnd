using Domain.Commands;
using Domain.Commands.Courier;
using Domain.Handlers.Courier;
using Domain.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Utils;
using System.Text.RegularExpressions;

namespace UnitTests.Domain.Handlers
{
    public class UploadDrivingLicenseHandleTest
    {
        private readonly Mock<IUpdateDrivingLicenseService> _updateDrivingLicenseService;
        private readonly Mock<ILogger<UploadDrivingLicenseHandle>> _logger;

        const string VALID_CNPJ = "85737613000169";
        const string VALID_DRIVING_LICENSE = "23601605795";
        const string STORAGE_URL = "https://qwdasfwsdas.supabase.co/storage/v1/s3";

        public UploadDrivingLicenseHandleTest()
        {
            _updateDrivingLicenseService = new Mock<IUpdateDrivingLicenseService>();
            _logger = new Mock<ILogger<UploadDrivingLicenseHandle>>();
        }

        [Fact]
        public async void ShouldReturnUrlWhenUploadSuccesfully()
        {
            var cancellationToken = CancellationToken.None;
            var mockFile = new Mock<IFormFile>();
            var mockStream = new Mock<Stream>();
            var buffer = new byte[10];
            var fileName = "cnh.png";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Fake image content");
            writer.Flush();
            ms.Position = 0;

            mockFile.Setup(_ => _.FileName).Returns(fileName);
            mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);

            _updateDrivingLicenseService.Setup(service => service.UploadFileAsync(VALID_CNPJ, It.IsAny<Stream>(), It.IsAny<string>(), cancellationToken))
                .ReturnsAsync((string cnpj, Stream stream, string fileName, CancellationToken cancellationToken) => Either<Error, string>.RightValue($"{STORAGE_URL}/{fileName}"));

            var command = new UploadDrivingLicenseCommand
            {
                File = mockFile.Object
            };
            command.SetCourierCnpj(VALID_CNPJ);

            var handle = new UploadDrivingLicenseHandle(_updateDrivingLicenseService.Object, _logger.Object);

            var result = await handle.Handle(command, cancellationToken);

            Assert.True(result.Success);
            Assert.IsType<CommandResponseData<string>>(result);

            CommandResponseData<string> commandResponse = (CommandResponseData<string>)result;

            var regex = new Regex($@"^{STORAGE_URL}/driving-license-{VALID_CNPJ}-[a-fA-F0-9]{{8}}-[a-fA-F0-9]{{4}}-[a-fA-F0-9]{{4}}-[a-fA-F0-9]{{4}}-[a-fA-F0-9]{{12}}\.png$");
            Assert.Matches(regex, commandResponse.Data);
        }

        [Fact]
        public async void ShouldReturnMessageErrorlWhenAnExceptionOccurs()
        {
            var cancellationToken = CancellationToken.None;
            var mockFile = new Mock<IFormFile>();
            var mockStream = new Mock<Stream>();
            var buffer = new byte[10];
            var fileName = "cnh.png";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Fake image content");
            writer.Flush();
            ms.Position = 0;

            mockFile.Setup(_ => _.FileName).Returns(fileName);
            mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);

            _updateDrivingLicenseService.Setup(service => service.UploadFileAsync(VALID_CNPJ, It.IsAny<Stream>(), It.IsAny<string>(), cancellationToken))
                .ReturnsAsync(Either<Error, string>.LeftValue(new Error("An error occurred while uploading the driving license image")));

            var command = new UploadDrivingLicenseCommand
            {
                File = mockFile.Object
            };
            command.SetCourierCnpj(VALID_CNPJ);

            var handle = new UploadDrivingLicenseHandle(_updateDrivingLicenseService.Object, _logger.Object);

            var result = await handle.Handle(command, cancellationToken);

            Assert.False(result.Success);
            Assert.IsType<CommandResponseError>(result);

            CommandResponseError commandResponse = (CommandResponseError)result;

            Assert.Equal("An error occurred while uploading the driving license image", commandResponse.Message);
        }
    }
}
