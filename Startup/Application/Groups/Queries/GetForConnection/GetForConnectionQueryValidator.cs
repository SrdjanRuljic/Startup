using System;

namespace Application.Groups.Queries.GetForConnection
{
    public static class GetForConnectionQueryValidator
    {
        public static bool IsValid(this GetForConnectionQuery model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.ConnectionId))
            {
                validationMessage += Resources.Translation.ConnectionIdRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}