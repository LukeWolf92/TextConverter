using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace TextConverter
{       
    class Mqtt
    {
        public string MqttCfg = "";
        public void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // convert Byte[] into a readable string
            MqttCfg = System.Text.Encoding.Default.GetString(e.Message);                       
        }

        public void Subscribe( Settings settings )
        {                                    
            // create client instance
            MqttClient client = new MqttClient( settings.MqttIpAddress);

            // register to message received
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;            

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "software_config/YUMI_SERVER" with QoS 2
            client.Subscribe(new string[] { settings.MqttTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });            
        }
    }
}
