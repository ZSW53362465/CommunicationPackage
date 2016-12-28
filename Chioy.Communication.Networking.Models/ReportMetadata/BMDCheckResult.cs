using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chioy.Communication.Networking.Models.ReportMetadata
{
    public class BMDCheckResult : BaseCheckResult
    {
        public BMDCheckResult()
        { }
        public double Fracturerisk { get; set; }

        public string Position { get; set; }

        public int Percentage { get; set; }

        public string Physical { get; set; }

        public int Sos { get; set; }
        public double T_val { get; set; }

        public double Z_val { get; set; }

        public double HP { get; set; }

        public double STI { get; set; }
        public double EOA { get; set; }
        public double REF { get; set; }
        public double PAB { get; set; }
        public string LimbSide { get; set; }

    }
}
