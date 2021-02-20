using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Data.SqlClient;


namespace VulnerableConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = args?.FirstOrDefault();

            var cmdText = "SELECT * FROM Users WHERE username = '" + input + "' and role='user'";
            using var connection = new SqlConnection("connection string");

            var cmd = new SqlCommand(cmdText, connection);
            cmd.ExecuteNonQuery();

        }
    }
}
