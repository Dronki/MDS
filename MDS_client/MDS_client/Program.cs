using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MDS_client
{
    class Program
    {

        private static string clientID, clientKey, serverURL, postURL;
        private static int serverPort;

        private static string input;
        private static char[] sectionSplitter = { '[', ']'};
        static void Main(string[] args) {
            readConfig();
            Console.WriteLine("Using values: \n\tClientID: " + clientID
                                + ", \n\tClientKey: " + clientKey
                                + ", \n\tServer-URL: " + serverURL + ":" + serverPort);
            while (true) {
                input = readInput(input);
                //Console.WriteLine(input);
                pushMessage(input);
                input = "";
            }
        }

        /*
        * <summary>
        *   Push the message to the server.
        *   As well as construct the message, and create a timestamp.
        * </summary>
        */
        private static void pushMessage(string s) {
            DateTime time = new DateTime();
            string timeString = "";
            int priority = 0;
            string[] sections = s.Split(sectionSplitter);
            if (sections.Length > 1) {
                timeString = sections[1];
                time = Convert.ToDateTime(timeString);
                Console.WriteLine(time.ToString("yyyy-MM-dd HH:mm:ss"));
                priority = int.Parse(sections[3]);
                Console.WriteLine(priority);
                s = sections[4];
                s = s.Replace("\n", "<br />");
                Console.WriteLine(s);
            } else {
                time = DateTime.Now;
                priority = 0;
                s = sections[0];
                s = s.Replace("\n", "<br />");
                Console.WriteLine(s);
            }

            if (time != null && !string.IsNullOrWhiteSpace(s)) {
                string URI = serverURL + ":" + serverPort + postURL;
                Console.WriteLine(URI);
                string parameters = "clientID=" + clientID + "&clientKey=" + clientKey + "&message=" + s + "&prio=" + priority + "&time=" + time.ToString("yyyy-MM-dd HH:mm:ss");
                using (WebClient wc = new WebClient()) {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string HtmlResult = wc.UploadString(URI, parameters);
                    Console.WriteLine(HtmlResult);
                }
            }

        }

        /*
        * <summary>
        *   Read from the console-input, and return the value.
        * </summary>
        */
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
            postURL = ConfigurationManager.AppSettings["postURL"];
            serverPort = int.Parse(ConfigurationManager.AppSettings["serverPort"]);
        }
    }
}
