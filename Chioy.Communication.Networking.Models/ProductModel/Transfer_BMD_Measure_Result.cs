using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chioy.Communication.Networking.Models.ProductModel
{
    [DataContract]
    public class Transfer_BMD_Measure_Result : Entity
    {
        [DataMember]
        public int CheckID { get; set; }
        [DataMember]
        public int SOS { get; set; }
        [DataMember]
        public int Position { get; set; }
        [DataMember]
        public int Cycle { get; set; }
        [DataMember]
        public float TValue { get; set; }
        [DataMember]
        public float ZValue { get; set; }
        [DataMember]
        public int? LimbSide { get; set; }
        [DataMember]
        public int PercentValue { get; set; }
        [DataMember]
        public float RRF { get; set; }
        [DataMember]
        public float EOA { get; set; }
        [DataMember]
        public float PAB { get; set; }
        [DataMember]
        public float HP { get; set; }
        [DataMember]
        public float STI { get; set; }
    }
}
