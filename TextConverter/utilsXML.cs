using System.Collections.Generic;
using System.Xml;
using System;

namespace TextConverter
{
    class utilsXML
    {
        private static Measurements measurements = new Measurements();
        private static List<Measurements> measurementsList = new List<Measurements>();
        // USING FILE AND PATH FETCHED FROM CONFIG.XML
        public List<Measurements> readFromXML()
        {            
            string fileName = Program.configXML["inputFile"];
            string pathToXML = Program.configXML["inputPathDirectory"];
            string firstValue = Program.configXML["firstValue"];
            string lastValue = Program.configXML["lastValue"];
            string key_dict = "";

            XmlTextReader reader = new XmlTextReader(pathToXML + "\\" + fileName);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:         // The node is an element.                        
                        key_dict = reader.Name;
                        break;
                    case XmlNodeType.Text:            //Display the text in each element.     
                        {
                            if (key_dict == firstValue)
                            {
                                measurements = new Measurements();
                            }
                            StoreMeasurements(key_dict, reader.Value);                            
                            if (key_dict == lastValue)
                            {
                                measurementsList.Add(measurements);
                            }
                            break;
                        }                        
                    case XmlNodeType.EndElement:      //Display the end of the element.                       
                        break;
                }                                
            }
            return measurementsList;
        }

        // WHEN THE FILE AND THE PATH ARE FORCED IN
        public Dictionary<string, string> readFromXML(string fileName, string pathToXML)
        {
            string key_dict = "";
            var dictXML = new Dictionary<string, string>();
            XmlTextReader reader = new XmlTextReader(pathToXML + "\\" + fileName);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:         // The node is an element.                        
                        key_dict = reader.Name;
                        break;
                    case XmlNodeType.Text:            //Display the text in each element.                      
                        dictXML.Add(key_dict, reader.Value);
                        break;
                    case XmlNodeType.EndElement:      //Display the end of the element.                       
                        break;
                }
            }            
            return dictXML;
        }

        private static void StoreMeasurements(string key, string value)
        {
            switch (key.ToUpper())
            {
                case "TIMESTAMP":
                    measurements.TimeStamp = Convert.ToDateTime(value);
                    break;
                case "MACHINETYPE":
                    measurements.MachineType = value;
                    break;
                case "PART":
                    measurements.Part = value;
                    break;
                case "VALUEKIND":
                    measurements.ValueKind = value;
                    break;
                case "VALUE":
                    measurements.Value = Convert.ToDouble(value);
                    break;
                default:
                    break;
            }
        }
    }
}
