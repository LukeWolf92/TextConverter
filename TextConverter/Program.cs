using System;

namespace TextConverter
{
    class Program
    {                    
        static void Main(string[] args)
        {
            Settings settings = new Settings(); // initialize settings from config.xml

            Console.WriteLine("Subscribing to Mqtt Topic");
            
            MqttSubscriber mqttSubscriber = new MqttSubscriber();
            mqttSubscriber.Subscribe(settings);            
            System.Threading.Thread.Sleep(1000); // wait for event "Client_MqttMsgPublishReceived"

            Console.WriteLine("Storing Configuration from Topic");
            MqttCfgSettingsOrganiser mqttCfgSettings = new MqttCfgSettingsOrganiser(mqttSubscriber.MqttCfg);
            
            Transform transform = new Transform();
            transform.Start(settings, mqttCfgSettings);
        }       
    }    
}
