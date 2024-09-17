using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ABCMoneyTransfer.Data.Utilities
{
    public static class GeneralUtility
    {
        public static string GetUsernameFromClaim(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User;

            if (user == null) throw new Exception("No data found in claim.");

            if (user.Claims.All(x => x.Type != ClaimTypes.Name)) throw new Exception("No username in claim.");

            return user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        }

        public static DateTime GetCurrentNepaliDateTime()
        {
            return DateTime.UtcNow.AddMinutes(345);
        }

        public static string CombineName (string firstName, string? middleName, string lastName)
        {
            string fullName = firstName;

            // Add middle name if it's not null or empty
            if (!string.IsNullOrWhiteSpace(middleName))
            {
                fullName += " " + middleName;
            }

            // Add last name
            fullName += " " + lastName;

            return fullName;
        }

    }
}
