using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace TextConverter
{
    class Transform
    {
        private static readonly UtilsXML handleXML = new UtilsXML();
        private static readonly UtilsText handleText = new UtilsText();

        public void Start( Settings settings, string MqttCfg )
        {            
            List<Measurements> measurementsList = new List<Measurements>();

            while (true)
            {
                Console.WriteLine("Reading from input");
                measurementsList = readingFromInput(settings);
                Console.WriteLine("Writing into output");
                writingIntoOutput(settings, measurementsList);
                Console.WriteLine("Delay for refresh: " + settings.refreshTime / 1000 + "seconds");
                System.Threading.Thread.Sleep(settings.refreshTime);
                Console.WriteLine("Cleaning cache and starting a new cycle");
                measurementsList.Clear();
            }
        }

        // --------------------- RETRIEVING DATA FROM INPUT ---------------------
        private static List<Measurements> readingFromInput(Settings settings)
        {
            List<Measurements> measurementsList = new List<Measurements>();
            try
            {
                measurementsList = new List<Measurements>();

                if (settings.fileFormat == "TXT")
                {
                    measurementsList = handleText.readFromTXT(settings);
                }
                else if (settings.fileFormat == "XML")
                {
                    measurementsList = handleXML.readFromXML(settings);
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
        private static void writingIntoOutput(Settings settings, List<Measurements> measurementsList)
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
                    File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\output.json", json);


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
