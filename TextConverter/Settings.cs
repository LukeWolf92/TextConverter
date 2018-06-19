using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextConverter
{
    public class Settings
    {
        private readonly Dictionary<string, string> configXML;        
        private readonly string currentDirectory = Directory.GetCurrentDirectory();
        // input
        public readonly string inputPathDirectory;
        public readonly string inputFile;
        public readonly string fileFormat; // extension (xml/txt)
        public readonly string inputFormat;
        public readonly string splitKeyValue;
        public readonly string splitVariables;
        public readonly string firstValue;
        public readonly string lastValue;
        public readonly string timeStampName;

        // output
        public readonly string outputPathDirectory;

        // settings        
        public readonly string MqttTopicSubscribe;
        public readonly string MqttIpAddressSubscribe;
        public readonly string MqttTopicPublish;
        public readonly string MqttIpAddressPublish;
        public readonly int refreshTime;



        public Settings()
        {
            UtilsXML handleXML = new UtilsXML();
            //configXML = handleXML.readFromXML("config.xml", currentDirectory);
            configXML = handleXML.readFromXML("config.xml", "C:\\Progetti\\TextConverter\\TextConverter");
            
            // input
            inputPathDirectory = configXML["inputPathDirectory"];
            inputFile = configXML["inputFile"];
            string[] inputFileSplitted = configXML["inputFile"].Split('.');
            fileFormat = inputFileSplitted[inputFileSplitted.GetLength(0) - 1].ToUpper();
            inputFormat = configXML["inputFormat"];
            splitKeyValue = configXML["splitKeyValue"];
            splitVariables = configXML["splitVariables"];
            firstValue = configXML["firstValue"].ToUpper();
            lastValue = configXML["lastValue"].ToUpper();
            timeStampName = configXML["timeStampName"].ToUpper();

            // output
            outputPathDirectory = configXML["outputPathDirectory"];

            // settings
            MqttTopicSubscribe = configXML["MqttTopicSubscribe"];
            MqttIpAddressSubscribe = configXML["MqttIpAddressSubscribe"];
            MqttTopicPublish = configXML["MqttTopicPublish"];
            MqttIpAddressPublish = configXML["MqttIpAddressPublish"];
            refreshTime = Convert.ToInt16(configXML["refreshTime"]);
        }
    }
}
