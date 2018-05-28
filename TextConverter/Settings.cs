using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextConverter
{
    public class Settings
    {
        public readonly Dictionary<string, string> configXML;        
        public readonly string fileName;
        public readonly string fileFormat;
        public readonly string outputPath;
        public readonly int refreshTime;
        public readonly string MqttTopic;
        public readonly string MqttIpAddress;
        public readonly string splitKeyValue;
        public readonly string splitVariables;
        public readonly string firstValue;
        public readonly string lastValue;
        public readonly string inputFile;
        public readonly string inputPathDirectory;
        public readonly string inputFormat;
        public readonly string pathToXML;


        public Settings()
        {
            UtilsXML handleXML = new UtilsXML();

            configXML = handleXML.readFromXML("config.xml", "C:\\Progetti\\TextConverter\\TextConverter");

            refreshTime = Convert.ToInt16(configXML["refreshTime"]);
            fileName = configXML["inputFile"];
            string[] fileNameSplitted = configXML["inputFile"].Split('.');
            fileFormat = fileNameSplitted[fileNameSplitted.GetLength(0) - 1].ToUpper();
            outputPath = configXML["outputPathDirectory"];
            MqttTopic = configXML["MqttTopic"];
            MqttIpAddress = configXML["MqttIpAddress"];
            splitKeyValue = configXML["splitKeyValue"];
            splitVariables = configXML["splitVariables"];
            firstValue = configXML["firstValue"].ToUpper();
            lastValue = configXML["lastValue"].ToUpper();
            inputFile = configXML["inputFile"];
            inputPathDirectory = configXML["inputPathDirectory"];
            inputFormat = configXML["inputFormat"];
            pathToXML = configXML["inputPathDirectory"];
        }
    }
}
