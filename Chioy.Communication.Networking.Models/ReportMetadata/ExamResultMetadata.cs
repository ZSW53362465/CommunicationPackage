using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chioy.Communication.Networking.Models.ReportMetadata
{
    public enum ProductType
    {
        BMD = 0,
        KRTCD
    }

    public enum CardType
    {
        IDCard = 1,
        Passport,
        OfficialCard,
        Soldbuch,
        DrivingLicence
    }
    [XmlType("ReportMetadata")]
    public class ExamResultMetadata<T> where T : BaseCheckResult
    {
        private ProductType _type;

        public ExamResultMetadata(ProductType type)
        {
            this._type = type;
        }
        public ExamResultMetadata()
        { }
        public ProductType Type { get { return this._type; } }

        public string ReportID { get; set; }

        public string Name { get; set; }

        public string BrithDay { get; set; }
        public int Sex { get; set; }
        public CardType CardType { get; set; }

        public string ID { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string MeasureTime { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public T CheckResult { get; set; }
    }
}
