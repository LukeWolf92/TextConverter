using System;
using System.Collections.Generic;

namespace TextConverter
{
    class Program
    {
        public static readonly utilsXML handleXML = new utilsXML();
        public static readonly utilsText handleText = new utilsText();
        public static readonly string currentPath = System.IO.Directory.GetCurrentDirectory();
        public static Dictionary<string, string> configXML;

        static void Main(string[] args)
        {                       
            Console.WriteLine("------------------TEXT CONVERTER PROGRAM----------------");
            configXML = handleXML.readFromXML("config.xml", currentPath);
            int refreshTime = Convert.ToInt16(configXML["refreshTime"]);
            List<Measurements> output = new List<Measurements>();

            // INIZIO LETTURA CICLICA
            List<Dictionary<string, string>> inputData = new List<Dictionary<string, string>>();
            while (true)
            {
                // --------------------- RETRIEVING DATA FROM INPUT ---------------------
                try
                {
                    inputData = new List<Dictionary<string, string>>(); // ripulisco l'oggetto creandomene uno nuovo
                    if (configXML["inputType"] == "txt")
                    {                        
                        inputData = handleText.readFromTXT();
                    }
                    else if (configXML["inputType"] == "xml") 
                    {                        
                        inputData = handleXML.readFromXML();
                    }
                }                
                catch (Exception ex1)
                {
                    Console.WriteLine("ERROR: Failed to read from input\n" + ex1);
                    throw;
                }

                // --------------------- GENERATING OUTPUT FILE --------------------- //
                if ( inputData.Capacity == 0 )
                {
                    Console.WriteLine("The input is empty, no output is going to be generated");
                }
                else 
                {
                    try
                    {
                        /* ------------------------------------------*
                         *      FARE PARTE DI SCRITTURA OUTPUT       *
                         *-------------------------------------------*/
                        Measurements measurements = new Measurements();
                        
                        Console.WriteLine("Sto scrivendo l'output...Numero di tests: " + (inputData.Capacity - 1));                        
                        foreach ( var dict in inputData )
                        {
                            measurements = new Measurements();
                            foreach ( var keyValue in dict )
                            {
                                Console.WriteLine(keyValue);
                                switch (keyValue.Key)
                                {
                                    case "TimeStamp":
                                        measurements.TimeStamp = Convert.ToDateTime(keyValue.Value);
                                        break;
                                    case "MachineType":
                                        measurements.MachineType = keyValue.Value;
                                        break;
                                    case "Part":
                                        measurements.Part = keyValue.Value;
                                        break;
                                    case "ValueKind":
                                        measurements.ValueKind = keyValue.Value;
                                        break;
                                    case "Value":
                                        measurements.Value = Convert.ToDouble(keyValue.Value);
                                        break;
                                    default:
                                        break;
                                }                                    
                            }
                            output.Add(measurements);
                        }
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("ERROR: Failed to store the output\n" + ex2);
                        throw;
                    }
                    int pippo = output.Capacity;
                }
                
                System.Threading.Thread.Sleep(refreshTime);
            }
        }
    }    
}
