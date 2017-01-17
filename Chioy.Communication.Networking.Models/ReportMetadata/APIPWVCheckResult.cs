using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Models.ReportMetadata
{
    public class APIPWVCheckResult : BaseCheckResult
    {
        [DefaultValue(0)]
        public int PWVR
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int PWVL
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int SBPRB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int SBPLB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int DBPRB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int DBPLB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int MBPRB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int MBPLB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int PPRB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int PPLB
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int SBPRA
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int SBPLA
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int DBPRA
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int DBPLA
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int MBPRA
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int MBPLA
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int PPRA
        {
            get;
            set;
        }
        [DefaultValue(0)]
        public int PPLA
        {
            get;
            set;
        }
        [DefaultValue("")]
        public string ABIR
        {
            get;
            set;
        }
        [DefaultValue("")]
        public string ABIL
        {
            get;
            set;
        }
        [DefaultValue("")]
        public string BAIR
        {
            get;
            set;
        }
        [DefaultValue("")]
        public string BAIL
        {
            get;
            set;
        }
    }
}
