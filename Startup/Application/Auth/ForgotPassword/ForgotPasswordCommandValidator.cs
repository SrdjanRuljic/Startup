using System;
using System.Text.RegularExpressions;

namespace Application.Auth.ForgotPassword
{
    public static class ForgotPasswordCommandValidator
    {
        public static bool IsValid(this ForgotPasswordCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (String.IsNullOrWhiteSpace(model.Email))
            {
                validationMessage += Resources.Translation.EmailRequired;
                isValid = false;
            }

            if (!model.Email.IsValidEmail())
            {
                validationMessage += string.Format(Resources.Translation.InvalidEmail, model.Email);
                isValid = false;
            }

            return isValid;
        }

        private static bool IsValidEmail(this string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" +
                             @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" +
                             @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }
    }
}