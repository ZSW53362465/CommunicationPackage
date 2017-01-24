using Chioy.Communication.Networking.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private RenderTargetBitmap _bitmap;

        public ExamResultMetadata(ProductType type)
        {
            this.Type = type;
        }
        public string ParamterName { get; set; }
        public ExamResultMetadata()
        { }
        [DefaultValue("")]
        public string PatientID { get; set; }
        [DefaultValue(0)]
        public ProductType Type { get; }
        [DefaultValue("")]
        public string ReportID { get; set; }
        [DefaultValue("")]
        public string Name { get; set; }
        [DefaultValue("")]
        public string BrithDay { get; set; }
        [DefaultValue(1)]
        public int Sex { get; set; }
        [DefaultValue(1)]
        public CardType CardType { get; set; }
        [DefaultValue("")]
        public string CardID { get; set; }
        [DefaultValue(0)]
        public int Age { get; set; }
        [DefaultValue("")]
        public string Phone { get; set; }
        [DefaultValue("")]
        public string Address { get; set; }
        [DefaultValue("")]
        public string MeasureTime { get; set; }
        [DefaultValue(0)]
        public int Weight { get; set; }
        [DefaultValue(0)]
        public int Height { get; set; }
        [DefaultValue("")]
        public string Report { get; set; }
        [DefaultValue("")]
        public string RequestDate { get; set; }
        [DefaultValue("")]
        public string RequestDoctor { get; set; }
        [DefaultValue("")]
        public string RequestDepartment { get; set; }
        [DefaultValue("")]
        public string ExamDepartement { get; set; }
        [DefaultValue("")]
        public string ExamDoctor { get; set; }
        [DefaultValue("")]
        public string DiagnosticianDoctor { get; set; }
        [DefaultValue("")]
        public string Diagnosis { get; set; }
        [DefaultValue("")]
        public string CheckResult { get; set; }
        [DefaultValue("")]
        public string CheckDate { get; set; }
        [DefaultValue("")]
        public string Paramters { get; set; }
        [XmlIgnore]
        public RenderTargetBitmap Bitmap { get { return _bitmap; } private set { _bitmap = value; } }

        public void SetBitmap(RenderTargetBitmap bitmap)
        {
            _bitmap = bitmap;
        }


        public T Result { get; set; }
    }
}
