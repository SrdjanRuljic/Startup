using System;

namespace Application.Auth.Queries.Login
{
    public static class LoginQueryValidator
    {
        public static bool IsValid(this LoginQuery model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (String.IsNullOrWhiteSpace(model.Username))
            {
                validationMessage += "Username is required. ";
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Password))
            {
                validationMessage += "Password is required. ";
                isValid = false;
            }

            return isValid;
        }
    }
}
