using System;

namespace Application.Auth.ConfirmEmail
{
    public static class ConfirmEmailCommandValidator
    {
        public static bool IsValid(this ConfirmEmailCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = "Model can not be null. ";
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.UserName))
            {
                validationMessage += "Username is required. ";
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Token))
            {
                validationMessage += "Token is required. ";
                isValid = false;
            }

            return isValid;
        }
    }
}
