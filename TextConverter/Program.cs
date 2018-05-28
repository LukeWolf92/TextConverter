using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextConverter
{
    class Program
    {                    
        static void Main(string[] args)
        {
            Settings settings = new Settings(); // initialize settings from config.xml
            
            Mqtt mqtt = new Mqtt();
            mqtt.Subscribe(settings);            
            System.Threading.Thread.Sleep(1000); // wait for event "Client_MqttMsgPublishReceived"

            Transform transform = new Transform();
            transform.Start(settings, mqtt.MqttCfg);
        }       
    }    
}
