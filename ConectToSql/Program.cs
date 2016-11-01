using System;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {

        string connectionString = (@"Data Source = LENOVO\SQLEXPRESS; Integrated Security = False; User ID = sa; Password = reppla; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite;Initial Catalog = chickubator; MultiSubnetFailover = False");

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
                command.CommandText =
                    "Insert into chickubator.telemetry (Data, Temp, Hum, Switch) VALUES (getdate(), 100, 55, 1)";
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