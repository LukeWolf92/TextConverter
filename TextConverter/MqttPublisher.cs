using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace TextConverter
{
    class MqttPublisher
    {
        public void Publish( string message, Settings settings )
        {
            // create client instance
            MqttClient client = new MqttClient(settings.MqttIpAddressPublish);

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.Publish(settings.MqttTopicPublish, Encoding.UTF8.GetBytes(message));

        }
    }
}
