﻿using System;

namespace Application.Messages.Commands.ReadMany
{
    public static class ReadManyMassagesCommandValidator
    {
        public static bool IsValid(this ReadManyMassagesCommand model, out string validationMessage)
        {
            validationMessage = null;
            bool isValid = true;

            if (model == null)
            {
                validationMessage = Resources.Translation.ModelCanNotBeNull;
                isValid = false;
            }

            if (String.IsNullOrWhiteSpace(model.RecipientUserId))
            {
                validationMessage += Resources.Translation.RecipientRequired;
                isValid = false;
            }

            return isValid;
        }
    }
}