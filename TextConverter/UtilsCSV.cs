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

        public List<Measurements> readFromDailyCSV(MqttCfgSettingsOrganiser mqttCfgSettings, string dateTime)
        {
            using (var reader = new StreamReader(mqttCfgSettings.InputPathDirectory + "/" + Transform.fileName + ".csv"))            
            {
                List<string[]> varNamesList = new List<string[]>();                

                // RETRIEVE ALL THE KEYS (VARIABLE NAMES) FROM THE FIRST 5 ROWS
                for (int i=0; i<5; i++)
                {                    
                    string[] varNamesArray = reader.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    varNamesList.Add(varNamesArray);                                        
                }


                List<Dictionary<string, string>> rawMeasurements = new List<Dictionary<string, string>>();
                Dictionary<string, string> productDict = new Dictionary<string, string>();
                Dictionary<string, string> valuesDict;                
                string[] excelRow;
                // READING ALL THE VALUES IN THE EXCEL FILE                
                while (!reader.EndOfStream)
                {
                    excelRow = reader.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries);                    
                    // FIRST ELEMENT OF VALUES IS TIMESTAMP
                    if (excelRow[0].Contains(":")) 
                    {
                        productDict = new Dictionary<string, string>();
                        int i = 0;
                        foreach (var varName in varNamesList[0])
                        {
                            productDict.Add(varName, excelRow[i]);
                            i++;
                        }
                    }
                    else
                    {                        
                        valuesDict = new Dictionary<string, string>();
                        foreach (var pippo in productDict)
                        {
                            valuesDict.Add(pippo.Key, pippo.Value);
                        }
                        
                        string COD_N = "COD-" + Convert.ToInt16(excelRow[1]);
                        int variableIndex=0;
                        switch (COD_N)
                        {
                            case "COD-1":
                                variableIndex = 1;
                                // Use VarNamesList[1]
                                break;
                            case "COD-2":
                                variableIndex = 2;
                                // Use VarNamesList[2]
                                break;
                            case "COD-4":
                                variableIndex = 3;
                                // Use VarNamesList[3]
                                break;
                            case "COD-5":
                                variableIndex = 4;
                                // Use VarNamesList[4]
                                break;
                            default:
                                Console.WriteLine("Invalid selection. Please check COD-Number");
                                variableIndex = 5; // out of range
                                break;
                        }

                        int i = 0;                        
                        foreach (var varName in varNamesList[variableIndex])
                        {
                            valuesDict.Add(varName, excelRow[i]);                            
                            i++;
                        }
                        rawMeasurements.Add(valuesDict);

                    }
                }
                insertIntoMeasurementsList(rawMeasurements, dateTime);
            }
            return measurementsList;
        }

        private void insertIntoMeasurementsList(List<Dictionary<string, string>> rawMeasurements, string dateTime)
        {
            DateTime oldDateTime = Convert.ToDateTime("01/01/2000 00:00:00");
            // INSERTING TIMESTAMP & PART TO ALL
            foreach (var measure in rawMeasurements)
            {
                measurements = new Measurements();
                measurements.TimeStamp = Convert.ToDateTime(dateTime + " " + measure["Hour"]);
                measurements.Part = measure["FILEPARAM"];
                measurements.PartNumber = Convert.ToInt32(measure["BATCH"]);
                if (oldDateTime != measurements.TimeStamp)
                {                    
                    // measurements.ValueKind = "LOTTO";
                    // measurements.Value = Convert.ToInt32(measure["BATCH"]);
                    oldDateTime = measurements.TimeStamp;
                }
                measurementsList.Add(measurements);
            }
            
            int cnt = 0;
            foreach (var measure in rawMeasurements)
            {
                measurementsList[cnt].ValueKind = measure["DESC"];
                bool IsPositive = checkAllTestsResult(measure);
                if (IsPositive == true)
                {
                    measurementsList[cnt].Value = 1;
                }
                else
                {
                    measurementsList[cnt].Value = -1;
                }
                cnt++;
            }

        }

        private static bool checkAllTestsResult(Dictionary<string, string> measure)
        {
            bool IsPositive = false;
            foreach (var pippo in measure)
            {            
                if ( pippo.Key.ToString().Contains("Esito") )
                {
                    if (pippo.Value == "-1")
                    {
                        // once a negative test has been found, everything will be negative
                        IsPositive = false;
                        break;
                    }                        
                    else
                        IsPositive = true;
                }
            }           
            return IsPositive;
        }
    }
}
