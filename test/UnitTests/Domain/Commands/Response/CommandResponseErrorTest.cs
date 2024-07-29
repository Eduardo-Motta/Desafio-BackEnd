using Domain.Commands;
using FluentValidation.Results;

namespace UnitTests.Domain.Commands.Response
{
    internal class User
    {
        public int Id { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CommandResponseTest
    {
        [Fact]
        public void ShouldReturnMessageError()
        {
            string messageErro = "A system failure has occurred";
            var response = new CommandResponseError(messageErro);

            Assert.False(response.Success);
            Assert.Equal(messageErro, response.Message);
        }

        [Fact]
        public void ShouldReturnErrorListWithPropertiesAndMessage()
        {
            IEnumerable<ValidationFailure> failures = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required"),
                new ValidationFailure("LastName", "LastName must have more than 2 characters"),
                new ValidationFailure("Cnpj", "Invalid Cnpj"),

            };
            var response = new CommandResponseErrors(failures);

            Assert.False(response.Success);
            Assert.Equal(failures.Count(), response.Errors.Count());
            Assert.Collection(response.Errors,
                error1 =>
                {
                    Assert.Equal("Name", error1.Property);
                    Assert.Equal("Name is required", error1.Message);
                    Assert.Null(error1.AttemptedValue);
                },
                error2 =>
                {
                    Assert.Equal("LastName", error2.Property);
                    Assert.Equal("LastName must have more than 2 characters", error2.Message);
                    Assert.Null(error2.AttemptedValue);
                },
                error3 =>
                {
                    Assert.Equal("Cnpj", error3.Property);
                    Assert.Equal("Invalid Cnpj", error3.Message);
                    Assert.Null(error3.AttemptedValue);
                }
            );
        }

        [Fact]
        public void ShouldReturnIdAsSuccess()
        {
            Guid id = Guid.NewGuid();
            var response = new CommandResponseData<Guid>(id);

            Assert.True(response.Success);
            Assert.Equal(id, response.Data);
        }

        [Fact]
        public void ShouldReturnUserAsSuccess()
        {
            User user = new User
            {
                Id = 10,
                Fullname = "Linus Torvalds",
                Email = "contato@email.com",
                Password = "@Senha123"
            };

            var response = new CommandResponseData<User>(user);

            Assert.True(response.Success);
            Assert.IsType<User>(response.Data);
        }
    }
}
