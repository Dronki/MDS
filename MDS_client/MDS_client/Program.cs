using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MDS_client
{
    class Program
    {

        private static string clientID, clientKey, serverURL;

        private static string input;

        static void Main(string[] args) {
            readConfig();
            Console.WriteLine("Using values: \n\tClientID: " + clientID
                                + ", \n\tClientKey: " + clientKey
                                + ", \n\tServer-URL: " + serverURL);
            while (true) {
                input = readInput(input);
                Console.WriteLine(input);
                input = "";
            }
        }

        private static string readInput(string s) {
            return s = Console.ReadLine();
        }

        /*
        * <summary>
        *   Read the app.config file and populate values.
        * </summary>
        */
        private static void readConfig() {
            clientID = ConfigurationManager.AppSettings["clientID"];
            clientKey = ConfigurationManager.AppSettings["clientKey"];
            serverURL = ConfigurationManager.AppSettings["serverURL"];
        }
    }
}
