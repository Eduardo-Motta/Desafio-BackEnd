//using Domain.Commands.Courier;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services.Courier;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Utils;

namespace UnitTests.Domain.Services
{
    public class UpdateDrivingLicenseServiceTest
    {
        //    private readonly Mock<ILogger<UpdateDrivingLicenseService>> _loggerMock;
        //    private readonly Mock<ICourierRespository> _courierRepositoryMock;
        //    private readonly Mock<IStorageRepository> _storageRepositoryMock;

        //    public UpdateDrivingLicenseServiceTest()
        //    {
        //        _loggerMock = new Mock<ILogger<UpdateDrivingLicenseService>>();
        //        _courierRepositoryMock = new Mock<ICourierRespository>();
        //        _storageRepositoryMock = new Mock<IStorageRepository>();
        //    }

        //    [Fact]
        //    public async Task ShouldUploadDrivingLicenseImageAndReturnPublicLinkFromStorage()
        //    {
        //        var cnpj = "00976661000157";
        //        var drivingLicense = "13916392541";
        //        var cancellationToken = CancellationToken.None;
        //        var mockFile = new Mock<IFormFile>();
        //        var mockStream = new Mock<Stream>();
        //        var buffer = new byte[10];

        //        mockFile.Setup(_ => _.FileName).Returns("cnh.png");

        //        mockStream.Setup(s => s.Read(buffer, 0, buffer.Length)).Returns(5);

        //        var command = new UploadDrivingLicenseCommand { File = mockFile.Object };
        //        command.SetCourierCnpj(cnpj);

        //        string fileName = $"driving-license-{command.Cnpj}-{Guid.NewGuid}.png";

        //        var courier = new CourierEntity("Devon Dagg", cnpj, DateTime.Now.Date.AddYears(-45), drivingLicense, EDrivingLicenseCategory.B);

        //        _courierRepositoryMock
        //        .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
        //        .ReturnsAsync(courier);

        //        _storageRepositoryMock
        //        .Setup(repo => repo.UploadDrivingLicenseAsync(mockStream.Object, fileName, cancellationToken))
        //        .ReturnsAsync($"https://qwdasfwsdas.supabase.co/storage/v1/s3/{fileName}");

        //        var service = new UpdateDrivingLicenseService(_courierRepositoryMock.Object, _storageRepositoryMock.Object, _loggerMock.Object);

        //        var result = await service.UploadFileAsync(command.Cnpj, mockStream.Object, fileName, cancellationToken);

        //        Assert.True(result.IsRight);
        //        Assert.IsType<string>(result.Right);
        //        Assert.Equal($"https://qwdasfwsdas.supabase.co/storage/v1/s3/{fileName}", result.Right);
        //    }

        //    [Fact]
        //    public async Task ShouldReturnErrorWhenCnpjNotFound()
        //    {
        //        var cnpj = "97936986000148";
        //        var cancellationToken = CancellationToken.None;
        //        var mockFile = new Mock<IFormFile>();
        //        var mockStream = new Mock<Stream>();
        //        var buffer = new byte[10];

        //        mockFile.Setup(_ => _.FileName).Returns("cnh.png");

        //        mockStream.Setup(s => s.Read(buffer, 0, buffer.Length)).Returns(5);

        //        var command = new UploadDrivingLicenseCommand { File = mockFile.Object };
        //        command.SetCourierCnpj(cnpj);

        //        string fileName = $"driving-license-{command.Cnpj}-{Guid.NewGuid}.png";

        //        _courierRepositoryMock
        //        .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
        //        .ReturnsAsync((CourierEntity)null!);

        //        var service = new UpdateDrivingLicenseService(_courierRepositoryMock.Object, _storageRepositoryMock.Object, _loggerMock.Object);

        //        var result = await service.UploadFileAsync(command.Cnpj, mockStream.Object, fileName, cancellationToken);

        //        Assert.True(result.IsLeft);
        //        Assert.IsType<Error>(result.Left);
        //        Assert.Equal("Cnpj not found", result.Left.Message);
        //    }

        //    [Fact]
        //    public async Task ShouldNotUpdateDrivingLicenseAndReturnExceptionHandling()
        //    {
        //        var cnpj = "97936986000148";
        //        var cancellationToken = CancellationToken.None;
        //        var mockFile = new Mock<IFormFile>();
        //        var mockStream = new Mock<Stream>();
        //        var buffer = new byte[10];

        //        mockFile.Setup(_ => _.FileName).Returns("cnh.png");

        //        mockStream.Setup(s => s.Read(buffer, 0, buffer.Length)).Returns(5);

        //        var command = new UploadDrivingLicenseCommand { File = mockFile.Object };
        //        command.SetCourierCnpj(cnpj);

        //        string fileName = $"driving-license-{command.Cnpj}-{Guid.NewGuid}.png";

        //        _courierRepositoryMock
        //        .Setup(repo => repo.FindCourierByCnpj(cnpj, cancellationToken))
        //        .ThrowsAsync(new Exception("Repository exception"));

        //        var service = new UpdateDrivingLicenseService(_courierRepositoryMock.Object, _storageRepositoryMock.Object, _loggerMock.Object);

        //        var result = await service.UploadFileAsync(command.Cnpj, mockStream.Object, fileName, cancellationToken);

        //        Assert.True(result.IsLeft);
        //        Assert.IsType<Error>(result.Left);
        //        Assert.Equal("An error occurred while uploading the driving license image", result.Left.Message);
        //    }
    }
}
