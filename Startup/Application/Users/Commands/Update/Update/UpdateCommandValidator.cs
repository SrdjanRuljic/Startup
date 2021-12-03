﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Application.Users.Commands.Update.Update
{
    public static class UpdateCommandValidator
    {
        public static bool IsValid(this UpdateCommand model, out string validationMessage)
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

            if (model.Roles.Length == 0)
            {
                validationMessage += Resources.Translation.AtLeastOneRoleRequired;
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