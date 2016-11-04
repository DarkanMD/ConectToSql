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
        private static Arduino Rev3 = new Arduino();
        private static Sql Server = new Sql();

        static void Main()
        {
            Console.WriteLine("Let get the Party Started....");
            int interval = 30000;      //set timer interval
            myTimer = new Timer(interval);
            myTimer.Elapsed += new ElapsedEventHandler(MyEvent);
         //   myTimer.Elapsed += new ElapsedEventHandler(Server.OnceEveryFiveMinutes);  //trigering event after interval
            myTimer.AutoReset = true;           //"loop" the timer
            myTimer.Enabled = true;
            myTimer.Start();                    //start the timer
            GC.KeepAlive(myTimer);              //tell GC not to touch it(myTimer)

            Console.ReadLine();
        }

        static void MyEvent(object source, ElapsedEventArgs e)
        {
            Server.Message(Rev3.GetData(Rev3.GetPort()));
            Server.OnceEveryFiveMinutes();
            Console.WriteLine(Rev3.GetData(Rev3.GetPort()));                                   //test purpose only
        }

    }
}