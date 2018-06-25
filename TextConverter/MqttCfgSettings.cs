using System;
using System.Collections.Generic;
using System.Text;

namespace TextConverter
{
    class MqttCfgSettingsOrganiser
    {        
        public readonly string ConfigurationTopic;
        public readonly string Agent;
        public readonly string ServerUrl;
        public readonly string MqttTopic;
        public readonly string MqttTopicPublish;
        public readonly string MqttIpAddressTopicPublish;
        public readonly string GenerateFakeData;
        public readonly string InputPathDirectory;
        public readonly string InputFile;
        public readonly string InputFileFormat;
        public readonly string TimeStampName;
        public readonly int RefreshTime;

        // Variable parameters
        public readonly bool ForwardMeasure;
        public readonly string MachineType;
        public readonly string MachineModel;
        public readonly int MachineNumber;
        public readonly string Part;
        public readonly int PartNumber;
        public readonly int ReadClock;
        public readonly bool ReplaceUtcTime;
        public readonly bool RoundTimeStamp;
        public readonly bool StoreMeasure;

        // -------------- CREATING A READABLE VARIABLE FROM MQTT-CFG ------------
        public MqttCfgSettingsOrganiser(string MqttCfg)
        {
            Dictionary<string, string> MqttCfgDict = new Dictionary<string, string>();

            string[] MqttDataArray = MqttCfg.Split(',');
            foreach (string word in MqttDataArray)
            {
                string[] tempArray = word.TrimStart().Split(": ");

                for (int i=0; i<tempArray.Length;i++)
                {
                    // "Variables:"
                    if (tempArray[i].Contains("Variables"))
                    {
                        // skip over...
                    }
                    else
                    {
                        MqttCfgDict.Add(tempArray[i], tempArray[i + 1]);
                        i++;
                    }
                }
            }

            ConfigurationTopic = MqttCfgDict["ConfigurationTopic"];
            Agent = MqttCfgDict["Agent"];
            ServerUrl = MqttCfgDict["ServerUrl"];
            MqttTopic = MqttCfgDict["MqttTopic"];
            MqttTopicPublish = MqttCfgDict["MqttTopicPublish"];
            MqttIpAddressTopicPublish = MqttCfgDict["MqttIpAddressTopicPublish"];
            GenerateFakeData = MqttCfgDict["GenerateFakeData"];
            InputPathDirectory = MqttCfgDict["inputPathDirectory"];
            //InputPathDirectory.Replace("\\\\", "\\");
            InputFile = MqttCfgDict["inputFile"];
            string[] tempFileFormat = InputFile.Split('.');
            InputFileFormat = tempFileFormat[1].ToUpper();
            TimeStampName = MqttCfgDict["timeStampName"];
            RefreshTime = Convert.ToInt32( MqttCfgDict["refreshTime"] );

            // variable parameters
            ForwardMeasure = Convert.ToBoolean(MqttCfgDict["ForwardMeasure"]);
            MachineType = MqttCfgDict["MachineType"];
            MachineModel = MqttCfgDict["MachineModel"];
            MachineNumber = Convert.ToInt32(MqttCfgDict["MachineNumber"]);
            Part = MqttCfgDict["Part"];
            PartNumber = Convert.ToInt32(MqttCfgDict["PartNumber"]);
            ReadClock = Convert.ToInt32(MqttCfgDict["ReadClock"]);
            ReplaceUtcTime = Convert.ToBoolean(MqttCfgDict["ReplaceUtcTime"]);
            RoundTimeStamp = Convert.ToBoolean(MqttCfgDict["RoundTimeStamp"]);
            StoreMeasure = Convert.ToBoolean(MqttCfgDict["StoreMeasure"]);

        }
    }
}
