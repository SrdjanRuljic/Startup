using System;

namespace Application.Groups.Commands.Insert
{
    public static class InsertGroupCommandValidator
    {
        public static bool IsValid(this InsertGroupCommand model, out string validationMessage)
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

            if (String.IsNullOrWhiteSpace(model.Name))
            {
                validationMessage += Resources.Translation.GroupNameRequired;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.UserName))
            {
                validationMessage += Resources.Translation.UsernameRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}