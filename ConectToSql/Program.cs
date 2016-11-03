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
            int interval = 100000;      //set timer interval
            myTimer = new Timer(interval);
            myTimer.Elapsed += new ElapsedEventHandler(Sql.OnceEveryFiveMinutes);  //trigering event after interval
            myTimer.AutoReset = true;           //"loop" the timer
            myTimer.Enabled = true;
            myTimer.Start();                    //start the timer
            GC.KeepAlive(myTimer);              //tell GC not to touch it(myTimer)

            Console.WriteLine(Arduino.GetData(Arduino.GetPort()));          //test purpose
            Console.WriteLine(ConfigurationManager.ConnectionStrings["server"].ToString());
            Console.ReadLine();
        }

    }
}