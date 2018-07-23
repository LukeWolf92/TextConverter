using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace TextConverter
{
    class Transform
    {
        private static readonly UtilsXML handleXML = new UtilsXML();
        private static readonly UtilsText handleText = new UtilsText();
        private static readonly UtilsCSV handleCSV = new UtilsCSV();          
        

        public void Start(SettingsFromXML settingsFromXML, MqttCfgSettingsOrganiser mqttCfgSettings, MqttPublisher mqttPublisher)
        {
            List<Measurements> measurementsList = new List<Measurements>();
            
            while (true)
            {
                Console.WriteLine("-------------------------------------------------------------");

                Console.WriteLine("Reading from input");
                measurementsList = readingFromInput(settingsFromXML, mqttCfgSettings);

                Console.WriteLine("Adding info from MQTT Cfg");
                measurementsList = InsertMachineDataFromMqttCfg(measurementsList, mqttCfgSettings);

                Console.WriteLine("Checking Duplicates & Storing into PostgreSQL Database & forwarding onto MQTT Topic");
                writingIntoOutput(measurementsList, mqttCfgSettings, mqttPublisher);

                Console.WriteLine("Waiting Cycle time for refresh: " + mqttCfgSettings.Cycle + " minutes");
                System.Threading.Thread.Sleep(mqttCfgSettings.Cycle * 60 * 1000); // from minutes to ms   
                
                Console.WriteLine("Cleaning cache and starting a new detection");

                measurementsList.Clear();
            }
        }

        // --------------------- RETRIEVING DATA FROM INPUT ---------------------
        private static List<Measurements> readingFromInput(SettingsFromXML settingsFromXML, MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            List<Measurements> measurementsList = new List<Measurements>();            
            try
            {
                measurementsList = new List<Measurements>();

                if (mqttCfgSettings.InputFileFormat == "TXT") 
                {
                    measurementsList = handleText.readFromTXT(settingsFromXML, mqttCfgSettings);                    
                }
                else if (mqttCfgSettings.InputFileFormat == "XML")
                {
                    measurementsList = handleXML.readFromXML(settingsFromXML, mqttCfgSettings);
                }
                else if (mqttCfgSettings.InputFileFormat == "CSV")
                {
                    string dateTime = retrieveDateTime(mqttCfgSettings);                    
                    measurementsList = handleCSV.testCSV(mqttCfgSettings, dateTime);
                }
            }
            catch (Exception ex1)
            {
                Console.WriteLine("ERROR: Failed to read from input:\n" + ex1);
                throw;
            }            
            return measurementsList;
        }

        /***********************************************
         *          INSERIMENTO DATI DAL CFG           *
         *                                             *
         ***********************************************/
        private static List<Measurements> InsertMachineDataFromMqttCfg ( List<Measurements> measurementsList, MqttCfgSettingsOrganiser mqttCfgSettings )
        {
            foreach ( var measurements in measurementsList )
            {
                measurements.ReplaceUtcTime = mqttCfgSettings.ReplaceUtcTime;                
                measurements.RoundTimeStamp = mqttCfgSettings.RoundTimeStamp;
                measurements.ReadClock = mqttCfgSettings.ReadClock;

                measurements.ForwardMeasure = mqttCfgSettings.ForwardMeasure;                
                measurements.StoreMeasure = mqttCfgSettings.StoreMeasure;

                // THE "PART" KEY IS BEING SUED FOR THE PRODUCT CODE 
                // measurements.Part = mqttCfgSettings.Part; 

                // This is going to be the Lot Number
                //measurements.PartNumber = mqttCfgSettings.PartNumber;

                measurements.MachineNumber = mqttCfgSettings.MachineNumber;
                measurements.MachineType = mqttCfgSettings.MachineType;
                measurements.MachineModel = mqttCfgSettings.MachineModel;
            }
            return measurementsList;
        }

        // --------------------- GENERATING OUTPUT FILE --------------------- //
        private static void writingIntoOutput(List<Measurements> measurementsList, MqttCfgSettingsOrganiser mqttCfgSettings, MqttPublisher mqttPublisher)
        {
            if (measurementsList.Capacity != 0)
            {           
                //File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\output.json", JsonConvert.SerializeObject(measurementsList));
                // Opens the SQL Connection to the Postgre Database
                PostgreSqlManager Sql = new PostgreSqlManager(mqttCfgSettings);
                int Duplicates = 0;
                int Stored = 0;
                foreach (var measure in measurementsList)
                {
                    string json = JsonConvert.SerializeObject(measure);
                    bool IsDuplicate = true;
                    // CHECK DUPLICATE & STORE INTO DB                    
                    try
                    {
                        IsDuplicate = Sql.CheckingDuplicate(measure, mqttCfgSettings);

                        if (IsDuplicate == false)
                            Sql.StoreIntoPostgreSQL(measure, mqttCfgSettings);                        
                    }
                    catch (Exception StoreIntoDb)
                    {
                        Console.WriteLine("ERROR: Failed to committ transaction on PostgreSQL DB:\n" + StoreIntoDb);
                    }                    

                    if (IsDuplicate == false)
                    {
                        Stored++;
                        // PUBLISHING ON MQTT TOPIC
                        try
                        {
                            mqttPublisher.Publish(json, mqttCfgSettings);            
                        }
                        catch (Exception Publish)
                        {
                            Console.WriteLine("ERROR: Failed to publish measurement on MQTT:\n" + Publish);
                        }
                    }
                    else
                    {
                        Duplicates++;
                    }
                }
                Console.WriteLine("Duplicate measurements found: " + Duplicates);
                Console.WriteLine("Measurements sent to MQTT topic: " + Stored);
            }
            else
            {                
                Console.WriteLine("The input is empty, no output is going to be generated at this cycle...");
            }
        }     
        
        private static string retrieveDateTime(MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            string dateTime;

            dateTime = mqttCfgSettings.InputFile;
            dateTime = dateTime.Remove(dateTime.IndexOf('.'));

            string year = dateTime.Substring(0,4);
            string month = dateTime.Substring(4, 2);
            string day = dateTime.Substring(6, 2);

            return dateTime = day + '/' + month + '/' + year;
        }
    }
}
