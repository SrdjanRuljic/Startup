using Microsoft.AspNetCore.Http;

namespace Application.Common.Excentions
{
    public static class ExceptionExcentions
    {
        public static void AddApplicationExcention(this HttpResponse respounse, string message)
        {
            respounse.Headers.Add("Application-Exception", message);
            respounse.Headers.Add("Access-Control-Expose-Headers", "Application-Exception");
            respounse.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddArgumentnExcention(this HttpResponse respounse, string message)
        {
            respounse.Headers.Add("Argument-Exception", message);
            respounse.Headers.Add("Access-Control-Expose-Headers", "Argument-Exception");
            respounse.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddNotFoundExcention(this HttpResponse respounse, string message)
        {
            respounse.Headers.Add("Not-Found-Exception", message);
            respounse.Headers.Add("Access-Control-Expose-Headers", "Not-Found-Exception");
            respounse.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddUnauthorisedExcention(this HttpResponse respounse, string message)
        {
            respounse.Headers.Add("Unauthorised-Exception", message);
            respounse.Headers.Add("Access-Control-Expose-Headers", "Unauthorised-Exception");
            respounse.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
