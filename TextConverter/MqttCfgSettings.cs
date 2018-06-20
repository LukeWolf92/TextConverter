using System;
using System.Collections.Generic;
using System.Text;

namespace TextConverter
{
    class MqttCfgSettings
    {
        //public readonly string MachineType;
        //public readonly string MachineModel;
        //public readonly string MachineNumber;
        public readonly string[] MqttData;

        // -------------- CREATING A READABLE VARIABLE FROM MQTT-CFG ------------
        public MqttCfgSettings(string MqttCfg)
        {
            //MqttCfg = MqttCfg.Replace(@"\","");            
            MqttData = MqttCfg.Split(',');
        }
    }
}
