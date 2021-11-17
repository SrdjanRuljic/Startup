using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Application.Auth.Register
{
    public static class RegisterCommandValidator
    {
        public static bool IsValid(this RegisterCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Username))
            {
                validationMessage += Resources.Translation.UsernameRequired;
                isValid = false;
            }

            if (!model.Username.All(x => Char.IsLetterOrDigit(x) || x == '-' || x == '.' || x == '_' || x == '@' || x == '+'))
            {
                validationMessage += string.Format(Resources.Translation.InvalidUserName, model.Username);
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Password))
            {
                validationMessage += Resources.Translation.PasswordRequired;
                isValid = false;
            }

            if (model.Password.Length < 6)
            {
                validationMessage += string.Format(Resources.Translation.PasswordTooShort, 6);
                isValid = false;
            }

            if (!model.Password.IsValidPassword())
            {
                validationMessage += Resources.Translation.PasswordMustContain;
                isValid = false;
            }

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

        private static bool IsValidPassword(this string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$";

            return Regex.IsMatch(password, pattern);
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
