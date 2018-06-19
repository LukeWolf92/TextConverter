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
        public List<Measurements> readFromCSV( Settings settings )
        {
            using (var reader = new StreamReader(settings.inputPathDirectory + "/" + settings.inputFile ))
            {
                List<string[]> list = new List<string[]>();                
                string[] variables = reader.ReadLine().Split(',');
                string[] values;
                string line = "";
                string timeStamp =""; // stored to be added into each measurements, at the end of the lecture
                int k = 0;

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    if ( line != "" ) 
                    {
                        values = line.Split(',');                                                
                        for (int i=0; i<variables.Length; i++)
                        {
                            measurements = new Measurements();
                            if ( variables[i] != "") // jump over when it reaches the end of the excel row
                            {
                                if (variables[i] == "DATA:") // DATA: e ORA:
                                {
                                    timeStamp = values[i] + " " + values[i + 1]; 
                                    i++;
                                }
                                else if (i < 8)
                                {
                                    if (variables[i] == "PEZZI FATTI:")
                                    {
                                        measurements.ValueKind = variables[i];
                                        measurements.Value = Convert.ToDouble(values[i]);
                                    }
                                    else
                                    {
                                        measurements.ValueKind = variables[i];
                                        measurements.TextValue = values[i];
                                    }
                                    measurementsList.Add(measurements);
                                }
                                else
                                {
                                    measurements.ValueKind = variables[i];
                                    measurements.Value = Convert.ToDouble(values[i]);
                                    measurementsList.Add(measurements);
                                }
                            }
                        }
                        for ( k = k; k<measurementsList.Count; k++ )
                        {
                            measurementsList[k].TimeStamp = Convert.ToDateTime(timeStamp);
                        }                        
                    }                    
                }
            }
            return measurementsList;
        }
    }
}
