using FluentValidation;

namespace Application.Commands.RentOut.Validations
{
    public class CreateRentOutValidation : AbstractValidator<CreateRentOutCommand>
    {
        public CreateRentOutValidation()
        {
        }
    }
}
