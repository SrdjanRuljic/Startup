using System;

namespace Application.Auth.Queries.Login
{
    public static class LoginQueryValidator
    {
        public static bool IsValid(this LoginQuery model, out string validationMessage)
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

            if (String.IsNullOrWhiteSpace(model.Password))
            {
                validationMessage += Resources.Translation.PasswordRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}
