using ConectToSql;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Timers;
using System.Configuration;

namespace chickubator
{

    class Program
    {
        private static Timer myTimer;  //create timer



        static void Main()
        {
            Arduino Rev3 = new Arduino();
            Sql Server = new Sql();
            int interval = 100000;      //set timer interval
            myTimer = new Timer(interval);
            myTimer.Elapsed += new ElapsedEventHandler(Server.OnceEveryFiveMinutes);  //trigering event after interval
            myTimer.AutoReset = true;           //"loop" the timer
            myTimer.Enabled = true;
            myTimer.Start();                    //start the timer
            GC.KeepAlive(myTimer);              //tell GC not to touch it(myTimer)

            Console.WriteLine(Rev3.GetData(Rev3.GetPort()));          //test purpose
            Console.WriteLine(ConfigurationManager.ConnectionStrings["server"].ToString());
            Console.ReadLine();
        }

    }
}