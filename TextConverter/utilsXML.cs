using System.Collections.Generic;
using System.Xml;

namespace TextConverter
{
    class utilsXML
    {
        // USING FILE AND PATH FETCHED FROM CONFIG.XML
        public List<Dictionary<string, string>> readFromXML()
        {            
            string fileName = Program.configXML["inputFile"];
            string pathToXML = Program.configXML["inputPathDirectory"];
            string firstXmlValue = Program.configXML["firstXmlValue"];
            string lastXmlValue = Program.configXML["lastXmlValue"];
            string key_dict = "";
            var dictXML = new Dictionary<string, string>();
            var PIPPO = new List<Dictionary<string, string>>();

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
                            if (key_dict == firstXmlValue)
                            {                                
                                dictXML = new Dictionary<string, string>();
                            }
                            dictXML.Add(key_dict, reader.Value);
                            if (key_dict == lastXmlValue)
                            {
                                PIPPO.Add(dictXML);
                            }
                            break;
                        }                        
                    case XmlNodeType.EndElement:      //Display the end of the element.                       
                        break;
                }                                
            }
            return PIPPO;
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
    }
}
