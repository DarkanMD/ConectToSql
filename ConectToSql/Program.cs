using System;
using System.Data;
using System.IO.Ports;
using System.Data.SqlClient;
using System.Timers;

namespace chickubator
{

    class Program

    {

        private static Timer myTimer;

        static void Main()
        {


            int interval = 100000;
            myTimer = new Timer(interval);
            myTimer.Elapsed += new ElapsedEventHandler(OnceEveryFiveMinutes);
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
            myTimer.Start();
            GC.KeepAlive(myTimer);


            Console.WriteLine(getData(getPort()));
            Console.ReadLine();
        }


        static void OnceEveryFiveMinutes(object source, ElapsedEventArgs e)
        {

            string connectionString = (@"Data Source = LENOVO\SQLEXPRESS; Integrated Security = False; User ID = sa; Password = reppla; Connect Timeout = 15; Encrypt = False; TrustServerCertificate = True; ApplicationIntent = ReadWrite;Initial Catalog = chickubator; MultiSubnetFailover = False");

            string reply = getData(getPort());
            string t, h, s;
            t = reply.Substring(0, 4);
            h = reply.Substring(5, 4);
            s = reply.Substring(10, 1);
            string queryText = ("Insert into chickubator.telemetry(Data, Temp, Hum, Switch) VALUES(getdate(),"+t+","+h+","+s+")");
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


        static string getData(string _s)
        {
            if (_s == "null")
            {
                throw new System.ArgumentException("Port not found");
            }
            else
            {
                SerialPort Arduino = new SerialPort(_s, 9600);  //asigning the working/curent port
                string reply;
                Arduino.Open();
                Arduino.Write("Data");
                reply = Arduino.ReadLine();
                Arduino.Close();

                return reply;
            }
        }

        static string getPort()
        {

            string[] ports = SerialPort.GetPortNames();      //array of ports to be checked

            foreach (string port in ports)               //Begin testing ports
            {

                SerialPort current = new SerialPort(port, 9600);
                current.Open();
                current.Write("Hello");             // sending the code
                string yo = current.ReadLine();   //reading reply

                if (yo == "Yellow\r")
                {
                    //Closing port and return the result
                    current.Close();
                    return port;
                }
                else { return "null"; }

            }
            return "null";

        }
    }


}