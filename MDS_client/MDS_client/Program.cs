using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

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
                string result = "";
                Console.WriteLine(URI);
                string parameters = "regMsg=&clientID=" + clientID + "&clientKey=" + clientKey + "&message=" + s + "&prio=" + priority.ToString() + "&time=" + time.ToString("yyyy-MM-dd&nbsp;HH:mm:ss");

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URI);
                req.KeepAlive = false;
                req.Method = "POST";
                req.ContentLength = parameters.Length;
                req.ContentType = "application/x-www-form-urlencoded";
                req.AllowWriteStreamBuffering = false;
                req.Timeout = System.Threading.Timeout.Infinite;
                req.ProtocolVersion = HttpVersion.Version10;
                ServicePointManager.DefaultConnectionLimit = 1000;

                StreamWriter stmWriter = null;

                try{
                    stmWriter = new StreamWriter(req.GetRequestStream());
                    stmWriter.Write(parameters);
                } catch (Exception e){
                    Console.WriteLine(e.Message);
                }
                finally {
                    stmWriter.Close();
                }

                //Skriva ut resultatet.
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8)) {
                    result = sr.ReadToEnd();
                    sr.Close();
                }

                Console.WriteLine(result);
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
