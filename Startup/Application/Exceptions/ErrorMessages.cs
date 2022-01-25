namespace Application.Exceptions
{
    public static class ErrorMessages
    {
        private static string emailNotSend;
        private static string forbidden;
        private static string incorrectUsernameOrPassword;
        private static string inernalServerError;
        private static string unauthorised;
        private static string userExists;

        public static string EmailNotSend
        {
            get { return emailNotSend = Resources.Translation.EmailNotSend; }
        }

        public static string Forbidden
        {
            get { return forbidden = Resources.Translation.Forbidden; }
        }

        public static string IncorrectUsernameOrPassword
        {
            get { return incorrectUsernameOrPassword = Resources.Translation.IncorrectUsernameOrPassword; }
        }

        public static string InernalServerError
        {
            get { return inernalServerError = Resources.Translation.InernalServerError; }
        }

        public static string Unauthorised
        {
            get { return unauthorised = Resources.Translation.Unauthorised; }
        }

        public static string UserExists
        {
            get { return userExists = Resources.Translation.UserExists; }
        }

        public static string CreateEntityWasNotFoundMessage(string name, object key)
        {
            return string.Format(Resources.Translation.EntityWasNotFound, name, key);
        }
    }
}