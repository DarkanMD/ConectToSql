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
            MyTimer(30000);                                      //start the timer with interval that will triger the read/write procces

          //  GC.KeepAlive(myTimer);              //tell GC not to touch it(myTimer)

            Console.ReadLine();
        }

        static void MyEvent(object source, ElapsedEventArgs e)
        {
            Server.Message(Rev3.GetData());
            Server.OnceEveryFiveMinutes();
            Console.WriteLine(Rev3.GetData());                                   //test purpose only
        }

        static void MyTimer(int interval)
        {
            myTimer = new Timer(interval);
            myTimer.Elapsed += new ElapsedEventHandler(MyEvent);
            //   myTimer.Elapsed += new ElapsedEventHandler(Server.OnceEveryFiveMinutes);  //trigering event after interval
            myTimer.AutoReset = true;           //"loop" the timer
            myTimer.Enabled = true;
            myTimer.Start();                    //start the timer

        }
    }
}