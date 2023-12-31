﻿namespace Application.Exceptions
{
    public static class ErrorMessages
    {
        private static string inernalServerError;
        private static string unauthorised;
        private static string dataNotFound;
        private static string incorectUsernameOrPassword;

        public static string InernalServerError
        {
            get { return inernalServerError = "Server side error, please contact the administrator. "; }
        }

        public static string Unauthorised
        {
            get { return inernalServerError = "Unauthorised. "; }
        }

        public static string IncorectUsernameOrPassword
        {
            get { return incorectUsernameOrPassword = "Incorect username or password. "; }
        }

        public static string DataNotFound
        {
            get { return dataNotFound = "The requested data was not found."; }
        }
    }
}
