using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Models.ReportMetadata
{
    public class TCDDopplerCheckResult:BaseCheckResult
    {
        [DefaultValue("")]
        public string StudyUID { get; set; }
        [DefaultValue(0)]
        public int Age { get; set; }
        [DefaultValue(1)]
        public int Gender { get; set; }
        [DefaultValue("")]
        public string ImageFilePath { get; set; }
        [DefaultValue("")]
        public string VesselName { get; set; }
        [DefaultValue("")]
        public string VesselChineseName { get; set; }
        [DefaultValue(0.00)]
        public double Vs { get; set; }
        [DefaultValue(0.00)]
        public double Vd { get; set; }
        [DefaultValue(0.00)]
        public double Vm { get; set; }
        [DefaultValue(0.00)]
        public double PI { get; set; }
        [DefaultValue(0.00)]
        public double RI { get; set; }
        [DefaultValue(0.00)]
        public double SD { get; set; }
        [DefaultValue(0)]
        public int HR { get; set; }
        [DefaultValue(0)]
        public int Depth { get; set; }
        [DefaultValue(0)]
        public int ProberDirection { get; set; }
        [DefaultValue(0)]
        public int VesselDirection { get; set; }
        [DefaultValue(0)]
        public int UseIndex { get; set; }
        [DefaultValue(0)]
        public int Number { get; set; }
        [DefaultValue("")]
        public string UID { get; set; }
        [DefaultValue(0)]
        public int ProbeType { get; set; }
        [DefaultValue(0.00)]
        public double ProbeHz { get; set; }
        [DefaultValue(0)]
        public int Scale { get; set; }
        [DefaultValue(0)]
        public int PRF { get; set; }
        [DefaultValue(0)]
        public int BaseLine { get; set; }
        [DefaultValue(false)]
        public bool GraphUse { get; set; }
        [DefaultValue(false)]
        public bool ListUse { get; set; }
        [DefaultValue(0.00)]
        public double SBI { get; set; }
        [DefaultValue(0)]
        public int Gain { get; set; }
        [DefaultValue(0)]
        public int Filter { get; set; }
    }
}
