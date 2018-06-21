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

        public void Start(Settings settings, MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            List<Measurements> measurementsList = new List<Measurements>();

            while (true)
            {
                Console.WriteLine("Reading from input");
                measurementsList = readingFromInput(settings, mqttCfgSettings);
                Console.WriteLine("Adding info from Mqtt Cfg");
                measurementsList = InsertMachineDataFromMqttCfg(measurementsList, mqttCfgSettings);
                Console.WriteLine("Writing into output");
                writingIntoOutput(settings, measurementsList);
                Console.WriteLine("Delay for refresh: " + settings.refreshTime / 1000 + "seconds");
                System.Threading.Thread.Sleep(settings.refreshTime);
                Console.WriteLine("Cleaning cache and starting a new cycle");
                measurementsList.Clear();
            }
        }

        // --------------------- RETRIEVING DATA FROM INPUT ---------------------
        private static List<Measurements> readingFromInput(Settings settings, MqttCfgSettingsOrganiser mqttCfgSettings)
        {
            List<Measurements> measurementsList = new List<Measurements>();            
            try
            {
                measurementsList = new List<Measurements>();

                if (settings.fileFormat == "TXT") 
                {
                    measurementsList = handleText.readFromTXT(settings);
                }
                else if (settings.fileFormat == "XML")
                {
                    measurementsList = handleXML.readFromXML(settings);
                }
                else if (settings.fileFormat == "CSV")
                {
                    measurementsList = handleCSV.readFromCSV(settings);
                }
            }
            catch (Exception ex1)
            {
                Console.WriteLine("ERROR: Failed to read from input:\n" + ex1);
                throw;
            }            
            return measurementsList;
        }

        // DA FAREEEEEEEEEEEE
        private static List<Measurements> InsertMachineDataFromMqttCfg ( List<Measurements> measurementsList, MqttCfgSettingsOrganiser mqttCfgSettings )
        {
            foreach ( var measurements in measurementsList )
            {
                measurements.ReplaceUtcTime = Convert.ToBoolean(mqttCfgSettings.MqttConfiguration["ReplaceUtcTime"]);
                measurements.RoundTimeStamp = Convert.ToBoolean(mqttCfgSettings.MqttConfiguration["RoundTimeStamp"]);
                measurements.ReadClock = Convert.ToInt16(mqttCfgSettings.MqttConfiguration["ReadClock"]);

                measurements.ForwardMeasure = Convert.ToBoolean(mqttCfgSettings.MqttConfiguration["ForwardMeasure"]);
                measurements.StoreMeasure = Convert.ToBoolean(mqttCfgSettings.MqttConfiguration["StoreMeasure"]);

                measurements.Part = mqttCfgSettings.MqttConfiguration["Part"];
                measurements.PartNumber = Convert.ToInt16(mqttCfgSettings.MqttConfiguration["PartNumber"]);

                measurements.MachineNumber = Convert.ToInt16(mqttCfgSettings.MqttConfiguration["MachineNumber"]);
                measurements.MachineType = mqttCfgSettings.MqttConfiguration["MachineType"];
                measurements.MachineModel = mqttCfgSettings.MqttConfiguration["MachineModel"];
            }
            return measurementsList;
        }

        // --------------------- GENERATING OUTPUT FILE --------------------- //
        private static void writingIntoOutput(Settings settings, List<Measurements> measurementsList)
        {

            if (measurementsList.Capacity != 0)
            {
                try
                {
                    /*********************************************
                     *                                           *
                     *            SCRITTURA IN JSON              *
                     *                                           *
                     *********************************************/
                    string json = JsonConvert.SerializeObject(measurementsList);
                    File.WriteAllText(Directory.GetCurrentDirectory() + "\\Output\\output.json", json);
                    MqttPublisher mqttPublisher = new MqttPublisher();
                    mqttPublisher.Publish(json, settings);
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
