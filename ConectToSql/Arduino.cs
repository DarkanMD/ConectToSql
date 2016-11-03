using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ConectToSql
{
    class Arduino
    {
        public static string GetData(string _s)
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

        public static string GetPort()
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
