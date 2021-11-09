using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Application.Users.Commands.Insert
{
    public static class InsertUserCommandValidator
    {
        public static bool IsValid(this InsertUserCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = "Model can not be null. ";
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Username))
            {
                validationMessage += "Username is required. ";
                isValid = false;
            }

            if (!model.Username.All(x => Char.IsLetterOrDigit(x) || x == '-' || x == '.' || x == '_' || x == '@' || x == '+'))
            {
                validationMessage += "The username can only contain letters or numbers. ";
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Email))
            {
                validationMessage += "Email is required. ";
                isValid = false;
            }

            if (!model.Email.IsValidEmail())
            {
                validationMessage += "Email is not valid. ";
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
