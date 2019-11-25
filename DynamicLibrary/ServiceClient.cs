using Dapper;
using System;
using System.Data.SqlClient;

namespace DynamicLibrary
{
    public class ServiceClient
    {
        public int Connect(int connectTimeout)
        {
            Console.WriteLine("Called Connect with value: " + connectTimeout);
            return GetRowCount();
        }

        private int GetRowCount()
        {
            string connectionString = @"Data Source=localhost;Initial Catalog=GSTFMSSarojFoundrySecured;Integrated Security=True;";

            string sql = "SELECT count(*) FROM m_accounts";
            int count = -1;
            using (var connection = new SqlConnection(connectionString))
            {
                count = connection.QueryFirst<int>(sql);
            }
            
            return count;
        }
    }
}
