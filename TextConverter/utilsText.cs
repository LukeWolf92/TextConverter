using System;
using System.IO;
using System.Collections.Generic;

namespace TextConverter
{
    class utilsText
    {        
        public List<Dictionary<string, string>> readFromTXT()
        {
            var inputListData = new List<Dictionary<string, string>>();
            // Retrieve the info from the config.xml
            string splitKeyValue = Program.configXML["splitKeyValue"];
            string splitVariables = Program.configXML["splitVariables"];
            string inputFile = Program.configXML["inputFile"];
            string inputPathDirectory = Program.configXML["inputPathDirectory"];
            string inputFormat = Program.configXML["inputFormat"];             


            switch (inputFormat)
            {
                case "separatedLines":
                    {
                        var lines = File.ReadLines(inputPathDirectory + "\\" + inputFile);
                        Dictionary<string, string> dictText = null;

                        foreach (var line in lines)
                        {
                            string[] tokens = line.Split(splitKeyValue); // Use the splitKeyValue from the config.xml

                            if (tokens[0].Contains("timestamp"))
                            {
                                if (dictText != null)
                                {
                                    inputListData.Add(dictText);
                                }
                                dictText = new Dictionary<string, string>();
                            }
                            dictText.Add(tokens[0], tokens[1]);
                        }
                        if (dictText != null)
                        {
                            inputListData.Add(dictText);
                        }
                    }
                    break;

                case "singleLine":
                    {
                        string content = File.ReadAllText(inputPathDirectory + "\\" + inputFile);
                        string[] lines = content.Split(splitVariables); // Uses the splitVariables from the config.xml
                        Dictionary<string, string> dictText = null;

                        foreach (var line in lines)
                        {
                            string[] tokens = line.Split(splitKeyValue); // Uses the splitKeyValue from the config.xml

                            if (tokens[0].Contains("timestamp"))
                            {
                                if (dictText != null)
                                {
                                    inputListData.Add(dictText);
                                }
                                dictText = new Dictionary<string, string>();
                            }
                            dictText.Add(tokens[0], tokens[1]);
                        }
                        if (dictText != null)
                        {
                            inputListData.Add(dictText);
                        }
                    }
                    break;

                default:
                    {
                        Console.WriteLine("The inputFormat is not a recognized one, please check the config.xml!");
                    }
                    break;

            }
            return inputListData;
        }
    }
}
