using Domain.Commands.Courier;
using Domain.Commands.Courier.Validations;
using Domain.Enums;

namespace UnitTests.Domain.Commands.Courier
{
    public class CreateCourierValidationTest
    {
        const string VALID_CNPJ = "66915996000173";
        const string VALID_DRIVING_LICENSE = "58483779258";
        const string INVALID_CNPJ = "06915996000173";
        const string INVALID_DRIVING_LICENSE = "2902525432000";

        [Fact]
        public async void ShouldReturnErrorWhenNameIsEmpty()
        {
            var command = new CreateCourierCommand
            {
                Name = string.Empty,
                BirthDate = DateTime.Now.Date.AddYears(-30),
                Cnpj = VALID_CNPJ,
                DrivingLicense = VALID_DRIVING_LICENSE,
                DrivingLicenseCategory = EDrivingLicenseCategory.A,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("Name", error1.PropertyName);
                Assert.Equal("Name is required", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldReturnErrorWhenNameIsNull()
        {
            var command = new CreateCourierCommand
            {
                Name = null!,
                BirthDate = DateTime.Now.Date.AddYears(-30),
                Cnpj = VALID_CNPJ,
                DrivingLicense = VALID_DRIVING_LICENSE,
                DrivingLicenseCategory = EDrivingLicenseCategory.A,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("Name", error1.PropertyName);
                Assert.Equal("Name is required", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldReturnErrorWhenAgeLessThanEighteen()
        {
            var command = new CreateCourierCommand
            {
                Name = "Arnold Williams",
                BirthDate = DateTime.Now.Date.AddYears(-17),
                Cnpj = VALID_CNPJ,
                DrivingLicense = VALID_DRIVING_LICENSE,
                DrivingLicenseCategory = EDrivingLicenseCategory.A,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("BirthDate", error1.PropertyName);
                Assert.Equal("Driver must be 18 years or older", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldReturnErrorWhenCnpjIsEmpty()
        {
            var command = new CreateCourierCommand
            {
                Name = "Michael Jones",
                BirthDate = DateTime.Now.Date.AddYears(-30),
                Cnpj = string.Empty,
                DrivingLicense = VALID_DRIVING_LICENSE,
                DrivingLicenseCategory = EDrivingLicenseCategory.A,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("Cnpj", error1.PropertyName);
                Assert.Equal("Cnpj is required", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldReturnErrorWhenDrivingLicenseIsEmpty()
        {
            var command = new CreateCourierCommand
            {
                Name = "David Brown",
                BirthDate = DateTime.Now.Date.AddYears(-45),
                Cnpj = VALID_CNPJ,
                DrivingLicense = string.Empty,
                DrivingLicenseCategory = EDrivingLicenseCategory.B,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("DrivingLicense", error1.PropertyName);
                Assert.Equal("DrivingLicense is required", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldReturnErrorWhenDrivingLicenseCategoryIsInvalid()
        {
            var command = new CreateCourierCommand
            {
                Name = "John Smith",
                BirthDate = DateTime.Now.Date.AddYears(-20),
                Cnpj = VALID_CNPJ,
                DrivingLicense = VALID_DRIVING_LICENSE,
                DrivingLicenseCategory = (EDrivingLicenseCategory)5,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("DrivingLicenseCategory", error1.PropertyName);
                Assert.Equal("Invalid DrivingLicenseCategory", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldReturnErrorWhenCnpjIsInvalid()
        {
            var command = new CreateCourierCommand
            {
                Name = "David Smith",
                BirthDate = DateTime.Now.Date.AddYears(-18),
                Cnpj = INVALID_CNPJ,
                DrivingLicense = VALID_DRIVING_LICENSE,
                DrivingLicenseCategory = EDrivingLicenseCategory.B,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("Cnpj", error1.PropertyName);
                Assert.Equal("Invalid Cnpj", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldReturnErrorWhenDrivingLicenseIsInvalid()
        {
            var command = new CreateCourierCommand
            {
                Name = "Charlie Miller",
                BirthDate = DateTime.Now.Date.AddYears(-18),
                Cnpj = VALID_CNPJ,
                DrivingLicense = INVALID_DRIVING_LICENSE,
                DrivingLicenseCategory = EDrivingLicenseCategory.A,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.False(validate.IsValid);
            Assert.Single(validate.Errors);
            Assert.Collection(validate.Errors, error1 =>
            {
                Assert.Equal("DrivingLicense", error1.PropertyName);
                Assert.Equal("Invalid DrivingLicense", error1.ErrorMessage);
            });
        }

        [Fact]
        public async void ShouldNotReturnErrorsWhenAllFieldsAreCorrect()
        {
            var command = new CreateCourierCommand
            {
                Name = "Jack Johnson",
                BirthDate = DateTime.Now.Date.AddYears(-22),
                Cnpj = VALID_CNPJ,
                DrivingLicense = VALID_DRIVING_LICENSE,
                DrivingLicenseCategory = EDrivingLicenseCategory.AB,
            };
            var validate = await new CreateCourierValidation().ValidateAsync(command);

            Assert.True(validate.IsValid);
            Assert.Empty(validate.Errors);
        }
    }
}
