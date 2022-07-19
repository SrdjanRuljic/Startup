using System;

namespace Application.Messages.Commands.Insert
{
    public static class InsertMessageCommandValidator
    {
        public static bool IsValid(this InsertMessageCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.RecipientUserName))
            {
                validationMessage += Resources.Translation.RecipientUserNameRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}