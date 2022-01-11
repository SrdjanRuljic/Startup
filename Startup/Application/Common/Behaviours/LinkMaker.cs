using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace Application.Common.Behaviours
{
    public static class LinkMaker
    {
        public static string CreateConfirmLink(string username, string clientUri, string token)
        {
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"username", username },
                {"token", token }
            };

            //Uri uri = new Uri("https://localhost:44363/api/Auth/confirm-email/");

            string link = QueryHelpers.AddQueryString(clientUri, param);

            return link;
        }

        public static string CreateResetPasswordLink(string email, string clientUri, string token)
        {
            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"email", email },
                {"token", token }
            };

            string link = QueryHelpers.AddQueryString(clientUri, param);

            return link;
        }
    }
}