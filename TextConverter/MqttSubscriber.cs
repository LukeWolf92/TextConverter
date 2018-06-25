using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace TextConverter
{       
    class MqttSubscriber
    {
        public string MqttCfg = "";
        public void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // convert Byte[] into a readable string
            MqttCfg = System.Text.Encoding.Default.GetString(e.Message);
            MqttCfg = CleanCfg(MqttCfg);
        }

        public string CleanCfg(string MqttCfg)
        {

            MqttCfg = MqttCfg.Replace("\"", "").Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "");
            return MqttCfg;
        }

        public void Subscribe( SettingsFromXML settingsFromXML )
        {                                    
            // create client instance
            MqttClient client = new MqttClient( settingsFromXML.MqttIpAddressSubscribe);

            // register to message received
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;            

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "software_config/TWINS_M166" with QoS 2
            client.Subscribe(new string[] { settingsFromXML.MqttTopicSubscribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });            
        }
    }
}
