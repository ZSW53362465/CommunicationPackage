using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Models.ReportMetadata
{
    public class BMDCheckResult : BaseCheckResult
    {
        public BMDCheckResult()
        { }
        [DefaultValue(0.00)]
        public double Fracturerisk { get; set; }
        [DefaultValue("")]
        public string Position { get; set; }
        [DefaultValue(0)]
        public int Percentage { get; set; }
        [DefaultValue("")]
        public string Physical { get; set; }
        [DefaultValue(0)]
        public int SOS { get; set; }
        [DefaultValue(0.00)]
        public double TValue { get; set; }
        [DefaultValue(0.00)]
        public double ZValue { get; set; }
        [DefaultValue(0.00)]
        public double HP { get; set; }
        [DefaultValue(0.00)]
        public double STI { get; set; }
        [DefaultValue(0.00)]
        public double EOA { get; set; }
        [DefaultValue(0.00)]
        public double RRF { get; set; }
        [DefaultValue(0.00)]
        public double PAB { get; set; }
        [DefaultValue("")]
        public string LimbSide { get; set; }
    }
}
