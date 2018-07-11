using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace TextConverter
{
    class UtilsCSV
    {
        private static Measurements measurements = new Measurements();
        private static List<Measurements> measurementsList = new List<Measurements>();
        private static List<List<Measurements>> listOfMeasurementsList = new List<List<Measurements>>();
        public List<Measurements> readFromCSV(MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            using (var reader = new StreamReader(mqttCfgSettings.InputPathDirectory + "/" + mqttCfgSettings.InputFile ))
            {
                List<string[]> list = new List<string[]>();                
                string[] variableNames = reader.ReadLine().Split(',');
                string[] values;
                string excelRow = "";
                string tempTimeStamp =""; // stores and add the timestamp of each row, at the end of the every "ReadLine()"
                string product = "";
                int k = 0;

                while (!reader.EndOfStream)
                {
                    excelRow = reader.ReadLine();
                    if ( excelRow != "" ) 
                    {                        
                        values = excelRow.Split(',');                                                
                        for (int i=0; i<variableNames.Length; i++)
                        {
                            measurements = new Measurements();
                            if ( variableNames[i] != "") // skip to next "," when content is empty
                            {
                                if (variableNames[i].ToUpper() == mqttCfgSettings.TimeStampName) // DATA: e ORA:
                                {
                                    tempTimeStamp = values[i] + " " + values[i + 1]; 
                                    i++;
                                }
                                else
                                {
                                    measurements.ValueKind = variableNames[i];
                                    try // if VALUE can be a NUMBER
                                    {
                                        measurements.Value = Convert.ToDouble(values[i]);
                                    }
                                    catch // otherwise IT MUST BE a STRING
                                    {
                                        measurements.TextValue = values[i];
                                    }
                                    if (measurements.ValueKind == mqttCfgSettings.Part)
                                    {
                                        product = values[i];
                                    }
                                        
                                    measurementsList.Add(measurements);
                                }
                            }
                        }
                        // at the end of each row, it adds the stored timestamp for the report
                        for ( k = k; k<measurementsList.Count; k++ )
                        {
                            measurementsList[k].TimeStamp = Convert.ToDateTime(tempTimeStamp);
                            measurementsList[k].Part = product;                            
                        }                        
                    }                    
                }
            }
            return measurementsList;
        }
    }
}
