using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Application.Users.Commands.Update.Self
{
    public static class UpdateSelfCommandValidator
    {
        public static bool IsValid(this UpdateSelfCommand model, out string validationMessage)
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
