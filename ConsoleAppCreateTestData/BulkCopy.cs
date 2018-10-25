using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleAppCreateTestData
{
    class BulkCopy
    {

        #region buikcopy
       public static void InsertTwo()
        {
            Console.WriteLine("using BulkCopy insert data.");
            Stopwatch sw = new Stopwatch();
            DataTable dt = GetTableSchema();
            string connString = "Server=.;Database=FrankTestDB222;User Id=sa;Password = Aa000000; ";
            int totalRow = 100000;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
                bulkCopy.DestinationTableName = "Product";
                bulkCopy.BatchSize = dt.Rows.Count;
                conn.Open(); 

                for (int i = 0; i < totalRow; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = Guid.NewGuid();
                    dr[1] = string.Format("produc name tttt", i);
                    dr[2] = (decimal)i;
                    dt.Rows.Add(dr);
                }
                if (dt != null && dt.Rows.Count != 0)
                {
                    sw.Start();
                    bulkCopy.WriteToServer(dt);
                    sw.Stop();
                }
                Console.WriteLine(string.Format("insert {0} records spends {1} ms.", totalRow, sw.ElapsedMilliseconds));
            }
        }
       public static DataTable GetTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("Id",typeof(Guid)),
        new DataColumn("Name",typeof(string)),
        new DataColumn("Price",typeof(decimal))});
            return dt;
        }
        #endregion
    }
}
