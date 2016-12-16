using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chioy.Communication.Networking.Models.ProductModel
{
    [Serializable]
    public partial class Transfer_PatientCheck
    {
        public Transfer_PatientCheck()
        { }
        #region Model
        private int _height;
        private int _weight;
        private string _name;
        private int _gender;
        private DateTime _birthday;
        private int _patientid;
        private DateTime _checkdate;
        private DateTime _requestdate;
        private int _checktype;
        private string _checkresult;
        private string _diagnosis;
        private int _checkcount;
        private string _diseasehistory;
        private string _riskfactors;
        private string _medicine;
        private string _patient_id;
        private string _requestdepartment;
        private string _requestdoctor;
        private string _operatedepartment;
        private string _operatedoctor;
        private string _diagnostician;
        private string _race;
        private string _nation;
        private string _address;
        private string _phone;
        private string _firstconsultation;
        private string _requestdepartmentstr;
        private string _examdepartmentstr;
        private string _diagnosticianstr;
        private string _requestdoctorstr;
        private string _examdoctorstr;
        private int _patienttype;
        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            set { _height = value; }
            get { return _height; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Weight
        {
            set { _weight = value; }
            get { return _weight; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Gender
        {
            set { _gender = value; }
            get { return _gender; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BirthDay
        {
            set { _birthday = value; }
            get { return _birthday; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PatientID
        {
            set { _patientid = value; }
            get { return _patientid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CheckDate
        {
            set { _checkdate = value; }
            get { return _checkdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RequestDate
        {
            set { _requestdate = value; }
            get { return _requestdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CheckType
        {
            set { _checktype = value; }
            get { return _checktype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CheckResult
        {
            set { _checkresult = value; }
            get { return _checkresult; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Diagnosis
        {
            set { _diagnosis = value; }
            get { return _diagnosis; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CheckCount
        {
            set { _checkcount = value; }
            get { return _checkcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DiseaseHistory
        {
            set { _diseasehistory = value; }
            get { return _diseasehistory; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RiskFactors
        {
            set { _riskfactors = value; }
            get { return _riskfactors; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Medicine
        {
            set { _medicine = value; }
            get { return _medicine; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Patient_ID
        {
            set { _patient_id = value; }
            get { return _patient_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RequestDepartment
        {
            set { _requestdepartment = value; }
            get { return _requestdepartment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RequestDoctor
        {
            set { _requestdoctor = value; }
            get { return _requestdoctor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperateDepartment
        {
            set { _operatedepartment = value; }
            get { return _operatedepartment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperateDoctor
        {
            set { _operatedoctor = value; }
            get { return _operatedoctor; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Diagnostician
        {
            set { _diagnostician = value; }
            get { return _diagnostician; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string race
        {
            set { _race = value; }
            get { return _race; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Nation
        {
            set { _nation = value; }
            get { return _nation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FirstConsultation
        {
            set { _firstconsultation = value; }
            get { return _firstconsultation; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RequestDepartmentStr
        {
            set { _requestdepartmentstr = value; }
            get { return _requestdepartmentstr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExamDepartmentStr
        {
            set { _examdepartmentstr = value; }
            get { return _examdepartmentstr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DiagnosticianStr
        {
            set { _diagnosticianstr = value; }
            get { return _diagnosticianstr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RequestDoctorStr
        {
            set { _requestdoctorstr = value; }
            get { return _requestdoctorstr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ExamDoctorStr
        {
            set { _examdoctorstr = value; }
            get { return _examdoctorstr; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PatientType
        {
            set { _patienttype = value; }
            get { return _patienttype; }
        }
        #endregion Model

    }
}
