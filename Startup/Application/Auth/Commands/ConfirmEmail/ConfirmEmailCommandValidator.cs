using System;

namespace Application.Auth.Commands.ConfirmEmail
{
    public static class ConfirmEmailCommandValidator
    {
        public static bool IsValid(this ConfirmEmailCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.UserName))
            {
                validationMessage += Resources.Translation.UsernameRequired;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Token))
            {
                validationMessage += Resources.Translation.TokenRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}