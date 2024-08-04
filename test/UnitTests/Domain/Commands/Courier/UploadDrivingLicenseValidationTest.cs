using Domain.Commands.Courier;
using Domain.Commands.Courier.Validations;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.Domain.Commands.Courier
{
    public class UploadDrivingLicenseValidationTest
    {
        [Fact]
        public async void ShouldReturnErrorWhenFileIsNull()
        {
            var command = new UploadDrivingLicenseCommand
            {
                File = null!,
            };

            var validate = await new UploadDrivingLicenseValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.NotEmpty(validate.Errors);
        }

        [Fact]
        public async Task ShouldReturnErrorWhenFileTypeIsInvalid()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(_ => _.FileName).Returns("cnh.jpg");

            var command = new UploadDrivingLicenseCommand { File = mockFile.Object };

            var validate = await new UploadDrivingLicenseValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.NotEmpty(validate.Errors);
        }

        [Fact]
        public async Task ShouldNotReturnErrorWhenFileTypeIsValid()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(_ => _.FileName).Returns("cnh.png");

            var command = new UploadDrivingLicenseCommand { File = mockFile.Object };

            var validate = await new UploadDrivingLicenseValidation().ValidateAsync(command);

            Assert.True(validate.IsValid);
            Assert.Empty(validate.Errors);
        }
    }
}
