using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Chioy.Communication.Networking.Models.DTO
{
    public class BMD_Measure_Result_DTO : Entity
    {
        public int CheckID { get; set; }
        public int SOS { get; set; }
        public int Position { get; set; }
        public int Cycle { get; set; }
        public float TValue { get; set; }
        public float ZValue { get; set; }
        public int? LimbSide { get; set; }
        public int PercentValue { get; set; }
        public float RRF { get; set; }
        public float EOA { get; set; }
        public float PAB { get; set; }
        public float HP { get; set; }
        public float STI { get; set; }
    }
}
