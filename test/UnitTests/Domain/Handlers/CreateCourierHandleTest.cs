//using Domain.Commands;
//using Domain.Commands.Courier;
//using Domain.Entities;
//using Domain.Enums;
//using Domain.Handlers.Courier;
//using Domain.Services.Contracts;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Shared.Utils;

namespace UnitTests.Domain.Handlers
{
    public class CreateCourierHandleTest
    {
        //private readonly Mock<ICreateCourierService> _createCourierService;
        //private readonly Mock<ILogger<CreateCourierHandle>> _logger;

        //const string VALID_CNPJ = "21750364000169";
        //const string VALID_DRIVING_LICENSE = "68306947660";

        //public CreateCourierHandleTest()
        //{
        //    _createCourierService = new Mock<ICreateCourierService>();
        //    _logger = new Mock<ILogger<CreateCourierHandle>>();
        //}

        //[Fact]
        //public async void ShouldReturnErrorsForFieldsNotProvided()
        //{
        //    var cancellationToken = CancellationToken.None;
        //    var command = new CreateCourierCommand
        //    {
        //        Name = string.Empty,
        //        BirthDate = DateTime.Now.Date.AddYears(-19),
        //        Cnpj = VALID_CNPJ,
        //        DrivingLicense = VALID_DRIVING_LICENSE,
        //        DrivingLicenseCategory = EDrivingLicenseCategory.A,
        //    };

        //    var handle = new CreateCourierHandle(_createCourierService.Object, _logger.Object);

        //    var result = await handle.Handle(command, cancellationToken);

        //    Assert.False(result.Success);
        //    Assert.IsType<CommandResponseErrors>(result);

        //    CommandResponseErrors commandResponse = (CommandResponseErrors)result;

        //    Assert.Single(commandResponse.Errors);
        //    Assert.Collection(commandResponse.Errors, error1 =>
        //    {
        //        Assert.Equal("Name", error1.Property);
        //        Assert.Equal("Name is required", error1.Message);
        //    });
        //}

        //[Fact]
        //public async void ShouldReturnErrorForRegistredCNPJ()
        //{
        //    var cancellationToken = CancellationToken.None;
        //    var command = new CreateCourierCommand
        //    {
        //        Name = "Edward Dewell",
        //        BirthDate = DateTime.Now.Date.AddYears(-27),
        //        Cnpj = VALID_CNPJ,
        //        DrivingLicense = VALID_DRIVING_LICENSE,
        //        DrivingLicenseCategory = EDrivingLicenseCategory.B,
        //    };

        //    var entity = new CourierEntity(command.Name, command.Cnpj, command.BirthDate.Date, command.DrivingLicense, command.DrivingLicenseCategory);

        //    _createCourierService
        //        .Setup(service => service.CreateCourier(It.IsAny<CourierEntity>(), cancellationToken))
        //        .ReturnsAsync(Either<Error, Guid>.LeftValue(new Error("Cnpj is already in use")));

        //    var handle = new CreateCourierHandle(_createCourierService.Object, _logger.Object);

        //    var result = await handle.Handle(command, cancellationToken);

        //    Assert.False(result.Success);
        //    Assert.IsType<CommandResponseError>(result);

        //    CommandResponseError commandResponse = (CommandResponseError)result;

        //    Assert.Equal("Cnpj is already in use", commandResponse.Message);
        //}

        //[Fact]
        //public async void ShouldReturnGuidForCreatedCourier()
        //{
        //    var cancellationToken = CancellationToken.None;
        //    var command = new CreateCourierCommand
        //    {
        //        Name = "Nerian Gibbard",
        //        BirthDate = DateTime.Now.Date.AddYears(-33),
        //        Cnpj = VALID_CNPJ,
        //        DrivingLicense = VALID_DRIVING_LICENSE,
        //        DrivingLicenseCategory = EDrivingLicenseCategory.AB,
        //    };

        //    CourierEntity capturedCourierEntity = null!;

        //    _createCourierService
        //        .Setup(service => service.CreateCourier(It.IsAny<CourierEntity>(), cancellationToken))
        //        .Callback<CourierEntity, CancellationToken>((courier, token) => capturedCourierEntity = courier)
        //        .ReturnsAsync((CourierEntity courier, CancellationToken token) => Either<Error, Guid>.RightValue(courier.Id));

        //    var handle = new CreateCourierHandle(_createCourierService.Object, _logger.Object);

        //    var result = await handle.Handle(command, cancellationToken);

        //    Assert.True(result.Success);
        //    Assert.IsType<CommandResponseData<Guid>>(result);

        //    CommandResponseData<Guid> commandResponse = (CommandResponseData<Guid>)result;

        //    Assert.NotEqual(Guid.Empty, commandResponse.Data);
        //    Assert.Equal(capturedCourierEntity.Id, commandResponse.Data);
        //}
    }
}
