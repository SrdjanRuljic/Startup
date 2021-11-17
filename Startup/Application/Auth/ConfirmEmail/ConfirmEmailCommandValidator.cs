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
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.UserName))
            {
                validationMessage += Resources.Translation.UsernameIsRequired;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Token))
            {
                validationMessage += Resources.Translation.TokenIsRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}
