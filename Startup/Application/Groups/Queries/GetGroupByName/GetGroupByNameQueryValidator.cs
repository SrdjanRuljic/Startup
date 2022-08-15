using System;

namespace Application.Groups.Queries.GetGroupByName
{
    public static class GetGroupByNameQueryValidator
    {
        public static bool IsValid(this GetGroupByNameQuery model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.Name))
            {
                validationMessage += Resources.Translation.GroupNameRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}