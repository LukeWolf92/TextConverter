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
        /// This value tells to the IbNet agent to store the value.
        /// </summary>
        public bool StoreMeasure { get; set; }

    }
}
