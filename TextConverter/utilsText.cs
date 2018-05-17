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

                Measurements.StoreMeasurements(tokens[0], tokens[1], measurements);                

                if (tokens[0].ToUpper() == lastValue)
                {
                    measurementsList.Add(measurements);
                }                    
            }
            return measurementsList;
        }
    }
}
