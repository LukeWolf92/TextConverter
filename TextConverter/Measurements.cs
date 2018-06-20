using System;
using System.Collections.Generic;
using System.Text;

namespace TextConverter
{
    /// <summary>
    /// Classe che rappresenta la misura da inviare
    /// </summary>
    public class Measurements
    {
        private double _value = double.NaN;

        /// <summary>
        /// Instant of acquisition of the value, in UTC format.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Machine that is providing the data. E.g. “HORIZONTAL-MILLING” or “RINGFRAME” etc.
        /// </summary>
        public string MachineType { get; set; }

        /// <summary>
        /// The specific model of the machine. E.g. “ZENITH-M” or “MDS1”
        /// </summary>
        public string MachineModel { get; set; }

        /// <summary>
        /// The incremental number of the machine. E.g. 1,2,3 etc
        /// </summary>
        public int MachineNumber { get; set; }

        /// <summary>
        /// The value kind of the measurement. E.g. “TEMPERATURE” or “CURRENT” or “POWER”
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// The incremental number of the part. E.g. 1,2,3 etc
        /// </summary>
        public int PartNumber { get; set; }

        /// <summary>
        /// The value kind of the measurement. E.g. “TEMPERATURE” or “CURRENT” or “POWER”
        /// </summary>
        public string ValueKind { get; set; }

        /// <summary>
        /// The value in double format (if the value is a number). If the value is a string it is expected that this property is set to “Nan” value (setted by default in the class);
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// The value in text format (if the value is a text). If the value is a number it is expected that this property is a Null value.
        /// </summary>
        public string TextValue { get; set; }

        /// <summary>
        /// This value tells to the cloud agent to round the timestamp to a multiple of the ReadClock property. >> Suggested value “True”
        /// </summary>
        public bool RoundTimeStamp { get; set; }

        /// <summary>
        /// This value is used to round the acquired timestamp to the read sampling time.
        /// </summary>
        public int ReadClock { get; set; }

        /// <summary>
        /// This value tells the cloud agent to replace the acquired timestamp with the utc time of the gateway pc >> Suggested value “True”
        /// </summary>
        public bool ReplaceUtcTime { get; set; }


        /// <summary>
        /// This value tells the cloud agent to forward the measurement to the cloud >> Suggested value “True”
        /// </summary>
        public bool ForwardMeasure { get; set; }

        /// <summary>
        /// This value tells to the agent to store the value.
        /// </summary>
        public bool StoreMeasure { get; set; }

        public static Measurements StoreMeasurements(string key, string value, Measurements measurements)
        {
            switch (key.ToUpper())
            {
                case "TIMESTAMP":
                    measurements.TimeStamp = Convert.ToDateTime(value);
                    break;
                case "MACHINETYPE":
                    measurements.MachineType = value;
                    break;
                case "MACHINEMODEL":
                    measurements.MachineModel = value;
                    break;
                case "MACHINENUMBER":
                    measurements.MachineNumber = Convert.ToInt32(value);
                    break;
                case "PART":
                    measurements.Part = value;
                    break;
                case "PARTNUMBER":
                    measurements.PartNumber = Convert.ToInt32(value);
                    break;
                case "VALUEKIND":
                    measurements.ValueKind = value;
                    break;
                case "TEXTVALUE":
                    measurements.TextValue = value;
                    break;
                case "VALUE":
                    measurements.Value = Convert.ToDouble(value);
                    break;
                case "ROUNDTIMESTAMP":
                    measurements.RoundTimeStamp = Convert.ToBoolean(value);
                    break;
                case "REPLACEUTCTIME":
                    measurements.ReplaceUtcTime = Convert.ToBoolean(value);
                    break;
                case "FORWARDMEASURE":
                    measurements.ForwardMeasure = Convert.ToBoolean(value);
                    break;
                case "STOREMEASURE":
                    measurements.StoreMeasure = Convert.ToBoolean(value);
                    break;
                case "READCLOCK":
                    measurements.ReadClock = Convert.ToInt32(value);
                    break;
                default:
                    Console.WriteLine("Couldn't recognize key: " + key);
                    break;
            }
            return measurements;
        }
    }

}
