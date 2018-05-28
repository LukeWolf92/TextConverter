using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextConverter
{
    class Program
    {
        public static Dictionary<string, string> configXML;        
        private static readonly utilsXML handleXML = new utilsXML();
        private static readonly utilsText handleText = new utilsText();
        private static readonly Mqtt mqtt = new Mqtt();
        private static readonly string currentDirectory = System.IO.Directory.GetCurrentDirectory();
        private static string[] fileName;
        private static string fileFormat;
        private static string outputPath;
        private static int refreshTime;        

        public static void Initializer()
        {                        
            configXML = handleXML.readFromXML("config.xml", currentDirectory);
            refreshTime = Convert.ToInt16(configXML["refreshTime"]);
            fileName = configXML["inputFile"].Split('.');
            fileFormat = fileName[fileName.GetLength(0) - 1].ToUpper();
            outputPath = configXML["outputPathDirectory"];
        }    

        static void Main(string[] args)
        {                       
            Initializer();
            List<Measurements> measurementsList = new List<Measurements>();

            // MQTT Subscribe
            mqtt.SubscribeAndReceiveMsg();
            // wait for event "Client_MqttMsgPublishReceived"
            System.Threading.Thread.Sleep(2000); 
            while ( true )
            {                
                

                Console.WriteLine("Reading from input");
                measurementsList = readingFromInput();
                Console.WriteLine("Writing into output");
                writingIntoOutput(measurementsList);
                Console.WriteLine("Delay for refresh: " + refreshTime / 1000 + "seconds");
                System.Threading.Thread.Sleep(refreshTime);
                Console.WriteLine("Cleaning cache and starting a new cycle");
                measurementsList.Clear();
            }
        }

        // --------------------- RETRIEVING DATA FROM INPUT ---------------------
        private static List<Measurements> readingFromInput()
        {
            List<Measurements> measurementsList = new List<Measurements>();            
            try
            {
                measurementsList = new List<Measurements>();

                if (fileFormat == "TXT")
                {
                    measurementsList = handleText.readFromTXT();
                }
                else if (fileFormat == "XML")
                {
                    measurementsList = handleXML.readFromXML();
                }
            }
            catch (Exception ex1)
            {
                Console.WriteLine("ERROR: Failed to read from input:\n" + ex1);
                throw;
            }
            return measurementsList;
        }

        // --------------------- GENERATING OUTPUT FILE --------------------- //
        private static void writingIntoOutput( List<Measurements> measurementsList)
        {
            
            if (measurementsList.Capacity != 0)
            {
                try
                {
                    /*********************************************
                     *       FARE PARTE DI SCRITTURA JSON        *
                     *                                           *
                     *********************************************/

                    string json = JsonConvert.SerializeObject(measurementsList);                    
                    File.WriteAllText(currentDirectory + "\\Output\\output.json", json);                    
                    

                }
                catch (Exception ex2)
                {
                    Console.WriteLine("ERROR: Failed to generate output file:\n" + ex2);
                    throw;
                }
            }
            else
            {
                Console.WriteLine("The input is empty, no output is going to be generated at this cycle.");
            }
        }
    }    
}
