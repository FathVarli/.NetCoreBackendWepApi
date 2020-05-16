using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DataAccess.Context
{
    public class DbInitializer
    {
        public static byte[] CreatePasswordHash(string password)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        }
        public static void Initialize(PostgresqlContext context)
        {
        }
    }
}
