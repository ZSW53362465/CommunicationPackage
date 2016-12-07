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
        public string Diagnose { get; set; }
        public double Fracturerisk { get; set; }

        public string Parts { get; set; }

        public int Percentage { get; set; }

        public string Physical { get; set; }

        public int Sos { get; set; }
        public double T_val { get; set; }

        public double Z_val { get; set; }
    }
}
