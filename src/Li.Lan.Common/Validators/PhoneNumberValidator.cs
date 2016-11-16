using Li.Lan.Common.Models;
using System.Text.RegularExpressions;

namespace Li.Lan.Common.Validators
{
    public class PhoneNumberValidator
    {
        private static readonly Regex NotNumbers = new Regex("[^0-9]*");

        public static ValidationResult<string> ValidatePhoneNumber(string phoneNumber)
        {
            var result = new ValidationResult<string>();
            result.Original = phoneNumber;
            result.IsValid = false;

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return result;

            var justNumbers = NotNumbers.Replace(phoneNumber, "");

            if (justNumbers.Length == 10)
            {
                var areaCode = justNumbers.Substring(0, 3);
                var part1 = justNumbers.Substring(3, 3);
                var part2 = justNumbers.Substring(6, 4);

                result.Result = string.Format("{0}-{1}-{2}", areaCode, part1, part2);

                result.IsValid = true;
                result.IsCorrected = (result.Original != result.Result);
            }
            else
            {
                result.IsValid = false;
                result.ValidationMessages.Add(new ValidationMessage() { Message = "Phone number did not contain enough numbers." });
            }

            return result;
        }
    }
}