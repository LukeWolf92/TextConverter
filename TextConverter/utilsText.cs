using System.IO;
using System.Collections.Generic;

namespace TextConverter
{
    class UtilsText
    {
        private static Measurements measurements = new Measurements();
        private static List<Measurements> measurementsList = new List<Measurements>();
        public List<Measurements> readFromTXT( SettingsFromXML settingsFromXML , MqttCfgSettingsOrganiser mqttCfgSettings)
        {                        
            // Retrieve the info from the config.xml
            string[] rows;

            if (settingsFromXML.inputFormat == "singleLine")
            {
                string content = File.ReadAllText(mqttCfgSettings.InputPathDirectory + "\\" + mqttCfgSettings.InputFile);
                rows = content.Split(settingsFromXML.splitVariables);
            }
            else // separatedRows
            {
                rows = File.ReadAllLines(mqttCfgSettings.InputPathDirectory + "\\" + mqttCfgSettings.InputFile);

            }
            foreach (var row in rows)
            {
                string[] tokens = row.Split(settingsFromXML.splitKeyValue);
                if (tokens[0].ToUpper() == settingsFromXML.firstValue)
                {
                    measurements = new Measurements();                    
                }

                Measurements.StoreMeasurements(tokens[0], tokens[1], measurements);                

                if (tokens[0].ToUpper() == settingsFromXML.lastValue)
                {
                    measurementsList.Add(measurements);
                }                    
            }
            return measurementsList;
        }
    }
}
