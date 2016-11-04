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

        string port = GetPort();
        public string GetData()        //accessing the board 
        {
            if (port == "null")
            {
                throw new System.ArgumentException("Port not found");       //in case we didn't find any ports
            }
            else
            {
                SerialPort Arduino = new SerialPort(port, 9600);  //asigning the working/curent port
                string reply;
                Arduino.Open();
                Arduino.Write("Data");
                reply = Arduino.ReadLine();
                double x;
                if (reply.Length !=11)
                {
                    throw new ArgumentException("Invalid message lenght");
                }
          /*      if (Double.TryParse(reply.Substring(0, 4),out x) || Double.TryParse(reply.Substring(5, 4), out x))
                {
                    throw new ArgumentException("Invalid data type");
                } */
                Arduino.Close();
                return reply;
            }
        }

        static string GetPort()
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
