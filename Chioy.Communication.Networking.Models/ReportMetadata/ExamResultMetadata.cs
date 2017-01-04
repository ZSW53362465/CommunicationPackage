using Chioy.Communication.Networking.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Chioy.Communication.Networking.Models.ReportMetadata
{


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
        private RenderTargetBitmap _bitmap;

        public ExamResultMetadata(ProductType type)
        {
            this._type = type;
        }
        public ExamResultMetadata()
        { }
        public string PatientID { get; set; }
        public ProductType Type { get { return this._type; } }

        public string ReportID { get; set; }

        public string Name { get; set; }

        public string BrithDay { get; set; }
        public int Sex { get; set; }
        public CardType CardType { get; set; }

        public string CardID { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string MeasureTime { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }

        public string Report { get; set; }

        public string RequestDate { get; set; }
        public string RequestDoctor { get; set; }
        public string RequestDepartment { get; set; }
        public string ExamDepartement { get; set; }
        public string ExamDoctor { get; set; }
        public string DiagnosticianDoctor { get; set; }
        public string Diagnosis { get; set; }
        public string CheckResult { get; set; }

        public string CheckDate { get; set; }

        public string Paramters { get; set; }
        [XmlIgnore]
        public RenderTargetBitmap Bitmap { get { return _bitmap; } private set { _bitmap = value; } }


        public T Result { get; set; }
    }
}
