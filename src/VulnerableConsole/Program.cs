using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.Data.SqlClient;


namespace VulnerableConsole
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            
            var input = args?.FirstOrDefault();

            var cmdText = "SELECT * FROM Users WHERE username = '" + input + "' and role='user'";
            using var connection = new SqlConnection("connection string");

            var cmd = new SqlCommand(cmdText, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
