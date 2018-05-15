using System;
using System.IO;
using System.Collections.Generic;

namespace TextConverter
{
    class utilsText
    {
        private static Measurements measurements = new Measurements();
        private static List<Measurements> measurementsList = new List<Measurements>();
        public List<Measurements> readFromTXT()
        {                      
            // Retrieve the info from the config.xml
            string splitKeyValue = Program.configXML["splitKeyValue"];
            string splitVariables = Program.configXML["splitVariables"];
            string firstValue = Program.configXML["firstValue"].ToUpper();
            string lastValue = Program.configXML["lastValue"].ToUpper();
            string inputFile = Program.configXML["inputFile"];
            string inputPathDirectory = Program.configXML["inputPathDirectory"];
            string inputFormat = Program.configXML["inputFormat"];
            string[] rows;

            if (inputFormat == "singleLine")
            {
                string content = File.ReadAllText(inputPathDirectory + "\\" + inputFile);
                rows = content.Split(splitVariables);
            }
            else // separatedRows
            {
                rows = File.ReadAllLines(inputPathDirectory + "\\" + inputFile);

            }
            foreach (var row in rows)
            {
                string[] tokens = row.Split(splitKeyValue);
                if (tokens[0].ToUpper() == firstValue)
                {
                    measurements = new Measurements();
                }                    

                StoreMeasurements(tokens[0], tokens[1]);

                if (tokens[0].ToUpper() == lastValue)
                {
                    measurementsList.Add(measurements);
                }                    
            }
            return measurementsList;
        }

        private static void StoreMeasurements(string key, string value)
        {
            switch (key.ToUpper())
            {
                case "TIMESTAMP":
                    measurements.TimeStamp = Convert.ToDateTime(value);
                    break;
                case "MACHINETYPE":
                    measurements.MachineType = value;
                    break;
                case "PART":
                    measurements.Part = value;
                    break;
                case "VALUEKIND":
                    measurements.ValueKind = value;
                    break;
                case "VALUE":
                    measurements.Value = Convert.ToDouble(value);
                    break;
                default:
                    break;
            }            
        }

    }
}
