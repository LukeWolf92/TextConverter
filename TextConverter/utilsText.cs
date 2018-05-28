using System.IO;
using System.Collections.Generic;

namespace TextConverter
{
    class UtilsText
    {
        private static Measurements measurements = new Measurements();
        private static List<Measurements> measurementsList = new List<Measurements>();
        public List<Measurements> readFromTXT( Settings settings )
        {                        
            // Retrieve the info from the config.xml
            string[] rows;

            if (settings.inputFormat == "singleLine")
            {
                string content = File.ReadAllText(settings.inputPathDirectory + "\\" + settings.inputFile);
                rows = content.Split(settings.splitVariables);
            }
            else // separatedRows
            {
                rows = File.ReadAllLines(settings.inputPathDirectory + "\\" + settings.inputFile);

            }
            foreach (var row in rows)
            {
                string[] tokens = row.Split(settings.splitKeyValue);
                if (tokens[0].ToUpper() == settings.firstValue)
                {
                    measurements = new Measurements();                    
                }

                Measurements.StoreMeasurements(tokens[0], tokens[1], measurements);                

                if (tokens[0].ToUpper() == settings.lastValue)
                {
                    measurementsList.Add(measurements);
                }                    
            }
            return measurementsList;
        }
    }
}
