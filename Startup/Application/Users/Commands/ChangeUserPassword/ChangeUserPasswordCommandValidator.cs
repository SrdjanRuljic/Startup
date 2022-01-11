using System;
using System.Text.RegularExpressions;

namespace Application.Users.Commands.ChangeUserPassword
{
    public static class ChangeUserPasswordCommandValidator
    {
        public static bool IsValid(this ChangeUserPasswordCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.NewPassword))
            {
                validationMessage += Resources.Translation.NewPasswordRequired;
                isValid = false;
            }

            if (!model.NewPassword.IsValidPassword())
            {
                validationMessage += Resources.Translation.NewPasswordMustContain;
                isValid = false;
            }

            if (String.Compare(model.NewPassword, model.ConfirmedPassword) != 0)
            {
                validationMessage += Resources.Translation.PasswordMismatch;
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
