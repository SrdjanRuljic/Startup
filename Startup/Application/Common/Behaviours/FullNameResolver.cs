using Domain.Entities.Identity;
using System.Globalization;

namespace Application.Common.Behaviours
{
    public static class FullNameResolver
    {
        public static string Resolve(AppUser source)
        {
            string fullName = string.Empty;

            if (source == null)
                return fullName;

            if (string.IsNullOrEmpty(source.FirstName) && string.IsNullOrEmpty(source.LastName))
                fullName = source.Email;
            else if (!string.IsNullOrEmpty(source.FirstName) && !string.IsNullOrEmpty(source.LastName))
                fullName = source.FirstName + " " + source.LastName;
            else if (!string.IsNullOrEmpty(source.FirstName) && string.IsNullOrEmpty(source.LastName))
                fullName = source.FirstName;
            else if (string.IsNullOrEmpty(source.FirstName) && !string.IsNullOrEmpty(source.LastName))
                fullName = source.LastName;

            return fullName;
        }

        public static string Resolve(string firstName, string lastName, string userName)
        {
            string fullName = string.Empty;

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && string.IsNullOrEmpty(userName))
                return fullName;

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                fullName = userName;
            else if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                fullName = firstName + " " + lastName;
            else if (!string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                fullName = firstName;
            else if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                fullName = lastName;

            return fullName;
        }
    }
}
