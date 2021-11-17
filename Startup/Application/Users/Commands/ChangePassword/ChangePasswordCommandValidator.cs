using System;
using System.Text.RegularExpressions;

namespace Application.Users.Commands.ChangePassword
{
    public static class ChangePasswordCommandValidator
    {
        public static bool IsValid(this ChangePasswordCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.CurrentPassword))
            {
                validationMessage += "Current password is required. ";
                isValid = false;
            }

            if (model.CurrentPassword.Length < 6)
            {
                validationMessage += "The current password must contain a minimum of 6 characters. ";
                isValid = false;
            }

            if (!model.CurrentPassword.IsValidPassword())
            {
                validationMessage += "The current password must contain uppercase and lowercase letters, numbers, and special characters. ";
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.NewPassword))
            {
                validationMessage += "New password is required. ";
                isValid = false;
            }

            if (model.NewPassword.Length < 6)
            {
                validationMessage += "The new password must contain a minimum of 6 characters. ";
                isValid = false;
            }

            if (!model.NewPassword.IsValidPassword())
            {
                validationMessage += "The new password must contain uppercase and lowercase letters, numbers, and special characters. ";
                isValid = false;
            }

            if (String.Compare(model.NewPassword, model.ConfirmedPassword) != 0)
            {
                validationMessage += "Confirmed password and new password are different. ";
                isValid = false;
            }

            return isValid;
        }

        private static bool IsValidPassword(this string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$";

            return Regex.IsMatch(password, pattern);
        }
    }
}
