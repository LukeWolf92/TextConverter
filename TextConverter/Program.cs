using System;
using System.Collections.Generic;

namespace TextConverter
{
    class Program
    {
        private static readonly utilsXML handleXML = new utilsXML();
        private static readonly utilsText handleText = new utilsText();
        public static Dictionary<string, string> configXML;
        private static Measurements measurements = new Measurements();
        private static List<Measurements> inputData = new List<Measurements>();
        private static string[] fileName;
        private static string fileFormat;
        private static int refreshTime;
        

        static void Main(string[] args)
        {                       
            Console.WriteLine("------------------TEXT CONVERTER PROGRAM----------------");
            // INITIALIZER
            Initializer();

            // INIZIO LETTURA CICLICA
            while (true)
            {
                // --------------------- RETRIEVING DATA FROM INPUT ---------------------
                try
                {
                    inputData = new List<Measurements>();

                    if (fileFormat == "TXT")
                    {                        
                        inputData = handleText.readFromTXT();
                    }
                    else if (fileFormat == "XML") 
                    {                        
                        inputData = handleXML.readFromXML();
                    }
                }                
                catch (Exception ex1)
                {
                    Console.WriteLine("ERROR: Failed to read from input:\n" + ex1);
                    throw;
                }

                // --------------------- GENERATING OUTPUT FILE --------------------- //
                if ( inputData.Capacity != 0 )
                {
                    /* ------------------------------------------*
                     *      FARE PARTE DI SCRITTURA OUTPUT       *
                     *-------------------------------------------*/
                }
                else 
                {
                    Console.WriteLine("The input is empty, no output is going to be generated");
                }
                
                // REFRESH TIMER
                System.Threading.Thread.Sleep(refreshTime);
                inputData.Clear();
            }
        }
        public static void Initializer()
        {
            configXML = handleXML.readFromXML("config.xml", System.IO.Directory.GetCurrentDirectory());
            refreshTime = Convert.ToInt16(configXML["refreshTime"]);
            fileName = configXML["inputFile"].Split('.');
            fileFormat = fileName[fileName.GetLength(0) - 1].ToUpper();
        }
    }    
}
