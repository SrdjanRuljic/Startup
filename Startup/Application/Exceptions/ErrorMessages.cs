namespace Application.Exceptions
{
    public static class ErrorMessages
    {
        private static string inernalServerError;
        private static string unauthorised;
        private static string dataNotFound;
        private static string incorrectUsernameOrPassword;
        private static string userExists;
        private static string emailNotSend;

        public static string InernalServerError
        {
            get { return inernalServerError = Resources.Translation.InernalServerError; }
        }

        public static string Unauthorised
        {
            get { return unauthorised = Resources.Translation.Unauthorised; }
        }

        public static string IncorrectUsernameOrPassword
        {
            get { return incorrectUsernameOrPassword = Resources.Translation.IncorrectUsernameOrPassword; }
        }

        public static string DataNotFound
        {
            get { return dataNotFound = Resources.Translation.DataNotFound; }
        }

        public static string UserExists
        {
            get { return userExists = Resources.Translation.UserExists; }
        }

        public static string EmailNotSend
        {
            get { return emailNotSend = Resources.Translation.EmailNotSend; }
        }
    }
}