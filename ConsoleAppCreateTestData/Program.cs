using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCreateTestData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("begin");
            //TestDB();
            BulkCopy.InsertTwo();
            Console.ReadKey();
        }


        private static void TestDB()
        {
            string connString = "Server=.;Database=FrankTestDB222;User Id=sa;Password = Aa000000; ";
            using (SqlCommand cmd = new SqlCommand())
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "select count(id) from t_EmailTicket_Ticket100014500";
                    var res = cmd.ExecuteScalar();
                    conn.Close();
                    Console.WriteLine(res);
                } 
               
            } 
        }
    }
}
