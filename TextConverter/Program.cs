﻿
namespace TextConverter
{
    class Program
    {                    
        static void Main(string[] args)
        {
            Settings settings = new Settings(); // initialize settings from config.xml
            
            MqttSubscriber mqttSubscriber = new MqttSubscriber();
            mqttSubscriber.Subscribe(settings);            
            System.Threading.Thread.Sleep(1000); // wait for event "Client_MqttMsgPublishReceived"

            MqttCfgSettings mqttCfgSettings = new MqttCfgSettings(mqttSubscriber.MqttCfg);
            
            Transform transform = new Transform();
            transform.Start(settings, mqttCfgSettings);
        }       
    }    
}
