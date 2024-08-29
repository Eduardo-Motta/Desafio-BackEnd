using System.Text.RegularExpressions;

namespace Application.Commands.Motorcycle.Validations
{
    public class LicensePlateValidation
    {
        public bool Validate(string licensePlate)
        {
            if (string.IsNullOrEmpty(licensePlate)) { return false; }

            if (licensePlate.Length != 7) { return false; }

            if (char.IsLetter(licensePlate, 4))
            {
                return ValidateMercosurStandard(licensePlate);
            }

            return ValidateBrazilianStandard(licensePlate);
        }

        private bool ValidateMercosurStandard(string licensePlate)
        {
            var mercosurStandard = new Regex("[a-zA-Z]{3}[0-9]{1}[a-zA-Z]{1}[0-9]{2}");
            return mercosurStandard.IsMatch(licensePlate);
        }

        private bool ValidateBrazilianStandard(string licensePlate)
        {
            var brazilianStandard = new Regex("[a-zA-Z]{3}[0-9]{4}");
            return brazilianStandard.IsMatch(licensePlate);
        }
    }
}
