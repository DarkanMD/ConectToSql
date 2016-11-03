using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConectToSql;
using System.Data.SqlClient;
using System.Timers;
using System.Configuration;

namespace ConectToSql
{
    class Sql
    {
        string reply;

        public Sql(string s)
        {
            reply = s;
        }
        public void OnceEveryFiveMinutes(object source, ElapsedEventArgs e)
        {
        
            //  string connectionString = (@"Data Source = LENOVO\SQLEXPRESS; Integrated Security = False; User ID = sa; Password = reppla; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite;Initial Catalog = chickubator; MultiSubnetFailover = False");
            string connectionString = ConfigurationManager.ConnectionStrings["server"].ToString();
            string t, h, s;
            t = reply.Substring(0, 4);      //temperature
            h = reply.Substring(5, 4);      //humidity
            s = reply.Substring(10, 1);     //switch (On/Off)
            string queryText = ("Insert into chickubator.telemetry(Data, Temp, Hum, Switch) VALUES(getdate()," + t + "," + h + "," + s + ")");      //create the command for SQL
            Console.WriteLine(queryText);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = queryText;
                    command.ExecuteNonQuery();
                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Record Is Writen");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    Console.ReadLine();

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
