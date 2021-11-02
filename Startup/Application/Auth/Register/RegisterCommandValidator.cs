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
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$";

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

            if (String.IsNullOrWhiteSpace(model.Password))
            {
                validationMessage += "Password is required. ";
                isValid = false;
            }

            if (model.Password.Length < 6)
            {
                validationMessage += "The password must contain a minimum of 6 characters. ";
                isValid = false;
            }

            if (!Regex.IsMatch(model.Password, passwordPattern))
            {
                validationMessage += "The password must contain uppercase and lowercase letters, numbers, and special characters. ";
                isValid = false;
            }

            return isValid;
        }
    }
}
