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

        public void Start(SettingsFromXML settingsFromXML, MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            List<Measurements> measurementsList = new List<Measurements>();

            while (true)
            {
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("Reading from input");
                measurementsList = readingFromInput(settingsFromXML, mqttCfgSettings);
                Console.WriteLine("Adding info from Mqtt Cfg");
                measurementsList = InsertMachineDataFromMqttCfg(measurementsList, mqttCfgSettings);
                Console.WriteLine("Writing into output");
                writingIntoOutput(measurementsList, mqttCfgSettings);
                Console.WriteLine("Delay for refresh: " + mqttCfgSettings.RefreshTime / 1000 + "seconds");
                System.Threading.Thread.Sleep(mqttCfgSettings.RefreshTime);
                Console.WriteLine("Cleaning cache and starting a new cycle");
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
                    measurementsList = handleCSV.readFromCSV(mqttCfgSettings);
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

                measurements.Part = mqttCfgSettings.Part;
                measurements.PartNumber = mqttCfgSettings.PartNumber;

                measurements.MachineNumber = mqttCfgSettings.MachineNumber;
                measurements.MachineType = mqttCfgSettings.MachineType;
                measurements.MachineModel = mqttCfgSettings.MachineModel;
            }
            return measurementsList;
        }

        // --------------------- GENERATING OUTPUT FILE --------------------- //
        private static void writingIntoOutput(List<Measurements> measurementsList, MqttCfgSettingsOrganiser mqttCfgSettings)
        {

            if (measurementsList.Capacity != 0)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(measurementsList);
                    File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\output.json", json);
                    MqttPublisher mqttPublisher = new MqttPublisher();
                    mqttPublisher.Publish(json, mqttCfgSettings);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("ERROR: Failed to generate output file:\n" + ex2);
                    throw;
                }
            }
            else
            {
                Console.WriteLine("The input is empty, no output is going to be generated at this cycle.");
            }
        }
    }
}
