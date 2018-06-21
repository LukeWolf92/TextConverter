using System;
using System.Collections.Generic;
using System.Text;

namespace TextConverter
{
    class MqttCfgSettingsOrganiser
    {
        public readonly Dictionary<string, string> MqttConfiguration = new Dictionary<string, string>();

        // -------------- CREATING A READABLE VARIABLE FROM MQTT-CFG ------------
        public MqttCfgSettingsOrganiser(string MqttCfg)
        {            
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
                        MqttConfiguration.Add(tempArray[i], tempArray[i + 1]);
                        i++;
                    }
                }
            }
        }
    }
}
