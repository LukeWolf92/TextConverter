using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextConverter
{
    public class SettingsFromXML
    {
        private readonly Dictionary<string, string> configXML;        
        private readonly string currentDirectory = Directory.GetCurrentDirectory();
        // input
        public readonly string inputFormat;
        public readonly string splitKeyValue;
        public readonly string splitVariables;
        public readonly string firstValue;
        public readonly string lastValue;

        // output
        public readonly string outputPathDirectory;

        // settings        
        public readonly string MqttTopicSubscribe;
        public readonly string MqttIpAddressSubscribe;



        public SettingsFromXML()
        {
            UtilsXML handleXML = new UtilsXML();
            //configXML = handleXML.readFromXML("config.xml", currentDirectory);
            configXML = handleXML.readFromXML("config.xml", "C:\\Progetti\\TextConverter\\TextConverter");

            // input
            inputFormat = configXML["inputFormat"];
            splitKeyValue = configXML["splitKeyValue"];
            splitVariables = configXML["splitVariables"];
            firstValue = configXML["firstValue"].ToUpper();
            lastValue = configXML["lastValue"].ToUpper();

            // output
            outputPathDirectory = configXML["outputPathDirectory"];

            // settings
            MqttTopicSubscribe = configXML["MqttTopicSubscribe"];
            MqttIpAddressSubscribe = configXML["MqttIpAddressSubscribe"];
        }
    }
}
